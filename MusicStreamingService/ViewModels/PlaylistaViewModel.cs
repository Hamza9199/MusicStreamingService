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
		public ICommand PlayPauseCommand { get; }

		private bool isPlaying;

		private Pjesma _currentSong;

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged2();
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
			OnPropertyChanged(nameof(CurrentSong));

			LoadSongsAsync();
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

		private async void OnPlayPause()
		{

			if (CurrentSong == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			var mediaManager = CrossMediaManager.Current;
			await mediaManager.Stop();


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

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged2([CallerMemberName] string propertyName = "")
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
