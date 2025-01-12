using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class OfflineMuzikaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public ObservableCollection<Pjesma> Pjesme { get; set; }

		private Pjesma _currentSong;

		private bool isPlaying;


		public ICommand PlayPauseCommand { get; }
		
		public ICommand Odi { get; }

		public ICommand ObrisiPjesmu { get; }

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged();
			}
		}

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
		public OfflineMuzikaViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			PlayPauseCommand = new Command(OnPlayPause);
			ObrisiPjesmu = new Command(OnObrisiPjesmu);
			Odi = new Command(OnOdi);

			//LoadAlbumAsync();
			LoadOfflineSongs();
		}

		public async void OnOdi()
		{
			if (CurrentSong == null)
			{
				return;
			}
			try
			{
				await App.Current.MainPage.Navigation.PushAsync(new AudioPlayer(CurrentSong));

			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom reprodukcije u OnPlayPause: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}
		}

		private async void OnObrisiPjesmu(object parameter)
		{
			var pjesmaZaBrisanje = parameter as Pjesma;

			if (pjesmaZaBrisanje == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			string audioPath = GetAudioPath(pjesmaZaBrisanje);

			if (!File.Exists(audioPath))
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Pjesma nije dostupna offline.", "U redu");
				return;
			}

			try
			{
				File.Delete(audioPath);
				Pjesme.Remove(pjesmaZaBrisanje);
				await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma obrisana!", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom brisanja pjesme: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu obrisati pjesmu.", "U redu");
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

			

			var mediaManager = CrossMediaManager.Current;
			await mediaManager.Stop();

			string audioPath = GetAudioPath(CurrentSong);

			if (!File.Exists(audioPath))
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Pjesma nije dostupna offline.", "U redu");
				return;
			}

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

		private void LoadOfflineSongs()
		{
			try
			{
				isLoading = true;

				string folderPath = FileSystem.AppDataDirectory;
				var files = Directory.GetFiles(folderPath, "*.mp3");

				foreach (var file in files)
				{
					Pjesme.Add(new Pjesma
					{
						naziv = Path.GetFileNameWithoutExtension(file),
						putanjaAudio = file,
						putanjaSlika = "Images/offlineslika.svg"
					});
				}
			}
			catch
			{
				Debug.WriteLine("Greška prilikom učitavanja offline pjesama.");
			}
			finally
			{
				isLoading = false;
			}
		}


		private async Task LoadAlbumAsync()
		{
			try
			{
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
						
							Pjesme.Add(pjesma);
						
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
	

}
