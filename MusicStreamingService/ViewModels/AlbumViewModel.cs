using CommunityToolkit.Maui.Views;
using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class AlbumViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public Models.Album CurrentAlbum { get; set; }
		private int CurrentSongIndex { get; set; } = 1;
		public ICommand PlayPauseCommand { get; }
		public ICommand ObrisiPjesmu { get; }

		public ICommand Play { get; }
		public ICommand Ponavljaj { get; }
		public ICommand Random { get; }
		public ICommand Dodaj { get; }

		private bool ponavljaj = false;
		private bool isPlaying;
		private Pjesma _currentSong;

		private System.Threading.Timer timer;
		private double currentPosition;
		private bool isLoading;
		public bool IsLoading
		{
			get => isLoading;
			set
			{
				isLoading = value;
				OnPropertyChanged(nameof(IsLoading));
			}
		}

		public bool _dozvoli = false;

		public bool _dozvoli2 = false;


		public bool Dozvoli
		{
			get => _dozvoli;
			set
			{
				if (_dozvoli != value)
				{
					_dozvoli = value;
					OnPropertyChanged();
				}
			}
		}

		public bool Dozvoli2
		{
			get => _dozvoli2;
			set
			{
				if (_dozvoli2 != value)
				{
					_dozvoli2 = value;
					OnPropertyChanged();
				}
			}
		}

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


		public DobiveniKorisnik CurrentKorisnik { get; set; }

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

		public AlbumViewModel(Models.Album odabraniAlbum)
		{
			CurrentAlbum = odabraniAlbum;

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			naziv = CurrentAlbum.naziv;
			Izvodjac = "Hamza";
			CurrentKorisnik = new DobiveniKorisnik();
			putanjaSlika = CurrentAlbum.putanjaSlika;
			PlayPauseCommand = new Command(OnPlayPause);
			Play = new Command(OnPlayAlbum);
			Ponavljaj = new Command(OnPonavljaj);
			Random = new Command(OnRandom);
			ObrisiPjesmu = new Command(OnObrisiPjesmu);
			Dodaj = new Command(OnDodajUPlaylistu);


			//CurrentSong = Pjesme[CurrentSongIndex];


			timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
			LoadTokenData();

			LoadAlbumAsync();
			UcitajDozvolu();
			//SubscribeToMediaManagerEvents();
		}

		private async void OnDodajUPlaylistu()
		{
			//await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma dodana u playlistu AL OZB", "U redu");

			try
			{
				var popup = new Poop(CurrentAlbum);
				await Shell.Current.CurrentPage.ShowPopupAsync(popup);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Greška", ex.Message, "U redu");
				Debug.WriteLine(ex.Message);
			}
		}

		private async void OnObrisiPjesmu()
		{
			if (CurrentSong == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}
			/*var response = await _httpClient.DeleteAsync($"api/PjesmaControllerAPI/{CurrentSong.id}");
			response.EnsureSuccessStatusCode();*/
			Pjesme.Remove(CurrentSong);
			await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma uspješno obrisana.", "U redu");

			await LoadAlbumAsync();
		}

		private async void LoadTokenData()
		{
			try
			{
				var tokenJson = await SecureStorage.GetAsync("token");
				if (!string.IsNullOrEmpty(tokenJson))
				{
					var token = System.Text.Json.JsonSerializer.Deserialize<DobiveniKorisnik>(tokenJson, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});

					if (token != null)
					{
						foreach (var claim in token.GetType().GetProperties())
						{
							Debug.WriteLine($"Claim: {claim.Name} = {claim.GetValue(token)}");
						}

						CurrentKorisnik.Id = token.Id;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private void UcitajDozvolu()
		{
			if(CurrentAlbum.korisnikid == CurrentKorisnik.Id)
			{
				Debug.WriteLine($"Uspjesno: {CurrentKorisnik.Id} - {CurrentAlbum.korisnikid}");
				Dozvoli = true;
				Dozvoli2 = true;
			}
			else
			{
				Debug.WriteLine($"Nusjesno: {CurrentKorisnik.Id} - {CurrentAlbum.korisnikid}");

				Dozvoli = false;
				Dozvoli2 = false;
			}
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
					if(CurrentSongIndex >= Pjesme.Count)
					{
						CurrentSongIndex = 0;
					}
					if(CurrentSongIndex < Pjesme.Count)
					{
						CurrentSong = Pjesme[CurrentSongIndex];
					}
					if(CurrentSong == null)
					{
						return;
					}
					if(CurrentSongIndex == null)
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

		private void Seek(int seconds)
		{
			CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(CurrentPosition + seconds));
		}


		private string _naziv;
		public string naziv
		{
			get => _naziv;
			set
			{
				_naziv = value;
				OnPropertyChanged();
			}
		}

		private string _izvodjac;
		public string Izvodjac
		{
			get => _izvodjac;
			set
			{
				_izvodjac = value;
				OnPropertyChanged();
			}
		}

		private string _putanjaSlika;
		public string putanjaSlika
		{
			get => _putanjaSlika;
			set
			{
				_putanjaSlika = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Pjesma> Pjesme { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		

		
	
		private string GetAudioPath(Pjesma pjesma)
		{
			if (pjesma == null)
			{
				System.Diagnostics.Debug.WriteLine("Greška: Nije selektovana nijedna pjesma.");
				return string.Empty;
			}

			return pjesma.putanjaAudio;
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
				System.Diagnostics.Debug.WriteLine($"Greška prilikom reprodukcije u OnPlayPause: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}


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

		private async Task LoadAlbumAsync()
		{
			try
			{
				isLoading = true;

				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				var pjesme = JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					Pjesme.Clear();

					foreach (var pjesma in pjesme)
					{
						if (pjesma.albumID == CurrentAlbum.id)
						{
							Pjesme.Add(pjesma);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
				if (CurrentAlbum.korisnikid == CurrentKorisnik.Id)
				{
					Debug.WriteLine($"Uspjesno: {CurrentKorisnik.Id} - {CurrentAlbum.korisnikid}");
					Dozvoli = true;
					Dozvoli2 = true;
				}
				else
				{
					Debug.WriteLine($"Nusjesno: {CurrentKorisnik.Id} - {CurrentAlbum.korisnikid}");

					Dozvoli = false;
					Dozvoli2 = false;
				}
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
