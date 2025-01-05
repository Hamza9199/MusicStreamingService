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
	public class AlbumViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public Models.Album CurrentAlbum { get; set; }

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
			putanjaSlika = CurrentAlbum.putanjaSlika;
			PlayPauseCommand = new Command(OnPlayPause);
			OnPropertyChanged(nameof(CurrentSong));
			LoadAlbumAsync();
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

		private string _izvodjac;
		public string Izvodjac
		{
			get => _izvodjac;
			set
			{
				_izvodjac = value;
				OnPropertyChanged(nameof(Izvodjac));
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

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged2([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async Task LoadAlbumAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

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
		}
	}
}
