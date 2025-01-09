using MediaManager;
using MusicStreamingService.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class PlaylistaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public Models.PlayLista CurrentPlaylista { get; set; }


		private int CurrentSongIndex { get; set; } = 0;

		public ICommand PlayPauseCommand { get; }

		public ICommand Play { get; }

		public ICommand Ponavljaj { get; }

		public ICommand Random { get; }

		private bool ponavljaj = false;

		private bool isPlaying;

		private Pjesma _currentSong;

		private System.Threading.Timer timer;
		private double currentPosition;

		public double CurrentPosition
		{
			get => currentPosition;
			set
			{
				if (value >= 0 && value <= SongDuration && Math.Abs(currentPosition - value) > 0.1)
				{
					currentPosition = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CurrentTime));
				}
			}
		}

		private double songDuration;

		public double SongDuration
		{
			get => songDuration;
			private set
			{
				songDuration = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(SongDurationString));
			}
		}

		public string SongDurationString => TimeSpan.FromSeconds(SongDuration).ToString(@"mm\:ss");



		public string CurrentTime => TimeSpan.FromSeconds(currentPosition).ToString(@"mm\:ss");

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged();
			}
		}
		public PlaylistaViewModel(Models.PlayLista odabranaPlaylista)
		{
			CurrentPlaylista = odabranaPlaylista;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			naziv = "Najbolje pjesme";
			KreiraoKorisnik = "Hamza";
			putanjaSlika = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUWPPJeKqMFiZdty1MgpNIUzPE0NYsz0Y0NA&s";
			PlayPauseCommand = new Command(OnPlayPause);
			Play = new Command(OnPlayAlbum);
			Ponavljaj = new Command(OnPonavljaj);
			Random = new Command(OnRandom);
			OnPropertyChanged(nameof(CurrentSong));


			timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

			LoadSongsAsync();
		}

		private async void TimerCallback(object state)
		{
			if (CrossMediaManager.Current.IsPlaying())
			{
				var position = CrossMediaManager.Current.Position;
				var duration = CrossMediaManager.Current.Duration;

				if ((position >= duration && duration > TimeSpan.Zero) ||
				(duration - position <= TimeSpan.FromSeconds(1) && duration > TimeSpan.Zero))
				{
					isPlaying = false;
					CurrentPosition = 0;
					CurrentSongIndex++;
					if (CurrentSongIndex >= Pjesme.Count)
					{
						CurrentSongIndex = 0;
					}
					if (CurrentSongIndex < Pjesme.Count)
					{
						CurrentSong = Pjesme[CurrentSongIndex];
					}
					if (CurrentSong == null)
					{
						return;
					}
					if (CurrentSongIndex == null)
					{
						return;
					}
					Thread.Sleep(1000);
					await PlaySongAtIndex(CurrentSongIndex);
					if (ponavljaj)
					{
						string audioPath = GetAudioPath(CurrentSong);
						CrossMediaManager.Current.Play(audioPath);
						isPlaying = true;
					}
					return;
				}

				if (position != TimeSpan.Zero && duration != TimeSpan.Zero)
				{
					CurrentPosition = position.TotalSeconds;
					SongDuration = duration.TotalSeconds;
				}



				OnPropertyChanged(nameof(CurrentTime));
				OnPropertyChanged(nameof(SongDurationString));
			}

		}

		private string _putanjaSlika;
		public string putanjaSlika
		{
			get => _putanjaSlika;
			set
			{
				_putanjaSlika = value;
				OnPropertyChanged(nameof(putanjaSlika));
			}
		}

		private string _naziv;
		public string naziv
		{
			get => _naziv;
			set
			{
				_naziv = value;
				OnPropertyChanged(nameof(naziv));
			}
		}

		private string _kreiraoKorisnik;
		public string KreiraoKorisnik
		{
			get => _kreiraoKorisnik;
			set
			{
				_kreiraoKorisnik = value;
				OnPropertyChanged(nameof(KreiraoKorisnik));
			}
		}


		private string GetAudioPath(Pjesma pjesma)
		{
			if (pjesma == null)
			{
				System.Diagnostics.Debug.WriteLine("Greška: Nije selektovana nijedna pjesma.");
				return string.Empty;
			}

			return pjesma.putanjaAudio;
		}


		private async void OnPlayAlbum()
		{
			if (Pjesme.Count == 0)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Album ne sadrži pjesme.", "U redu");
				return;
			}

			try
			{
				Thread.Sleep(1000);
				CurrentSongIndex = 0;
				await PlaySongAtIndex(CurrentSongIndex);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom reprodukcije u OnPlayAlbum: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}
		}

		private async void OnPonavljaj()
		{
			ponavljaj = !ponavljaj;
			
		}

		private async Task PlaySongAtIndex(int index)
		{
			try
			{
				if (index < 0 || index >= Pjesme.Count)
				{
					if (ponavljaj)
					{
						CurrentSongIndex = 0;
						await PlaySongAtIndex(CurrentSongIndex);
					}

					return;
				}


				Thread.Sleep(1000);
				CurrentSong = Pjesme[index];

				await PlaySongWithTimer(CurrentSong);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom reprodukcije u SongIndex: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}
		}

		private async Task PlaySongWithTimer(Pjesma song)
		{
			if (song == null)
			{
				Debug.WriteLine("Greška: Pjesma je null.");
				return;
			}

			try
			{
				var mediaManager = CrossMediaManager.Current;
				/*Thread.Sleep(1000);
				await mediaManager.Stop(); */
				string audioPath = GetAudioPath(song);
				Thread.Sleep(1000);

				await mediaManager.Play(audioPath);
				Debug.WriteLine($"Reprodukcija pjesme: {audioPath}");

				isPlaying = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom reprodukcije u SongTImer: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}
		}



		private async void OnRandom()
		{
			if (Pjesme.Count == 0)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Album ne sadrži pjesme.", "U redu");
				return;
			}

			var random = new Random();
			var randomIndex = random.Next(Pjesme.Count);
			CurrentSong = Pjesme[randomIndex];
		}

		

		private async void OnPlayPause()
		{

			if (CurrentSong == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			var mediaManager = CrossMediaManager.Current;
			await mediaManager.Stop();
			CurrentSongIndex = Pjesme.IndexOf(CurrentSong);

			string audioPath = GetAudioPath(CurrentSong);
			try
			{
				System.Diagnostics.Debug.WriteLine($"Pokušaj reprodukcije: {audioPath}");


				await mediaManager.Play(audioPath);



				isPlaying = true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom reprodukcije: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}


		}
		public ObservableCollection<Pjesma> Pjesme { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		

		private async Task LoadSongsAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var pjesmaPlaylista = await _httpClient.GetAsync($"api/PjesmaPlaylistaControllerAPI");
				pjesmaPlaylista.EnsureSuccessStatusCode();


				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var pjesmaPlaylistaJson = await pjesmaPlaylista.Content.ReadAsStringAsync();

				var pjesmaPlaylistaList = JsonSerializer.Deserialize<List<PjesmaPlayLista>>(pjesmaPlaylistaJson, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				var pjesme = JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					Pjesme.Clear();

					foreach (var pjesma in pjesme)
					{
						var povezanaPjesma = pjesmaPlaylistaList.Find(p => p.pjesmaID == pjesma.id && p.playlistaID == CurrentPlaylista.id);
						if (povezanaPjesma != null)
							Pjesme.Add(pjesma);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}
	}
}
