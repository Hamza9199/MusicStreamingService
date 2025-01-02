using MusicStreamingService.Models;
using MusicStreamingService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class KreirajPlaylistuViewModel : INotifyPropertyChanged
	{
		private PlayLista _playlista;
		private HttpClient _httpClient;
		public KreirajPlaylistuViewModel()
		{
			_playlista = new PlayLista();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial"))
			);

			SaveCommand = new Command(CreatePlaylista);
			PropertyChanged = delegate { };
		}
		public PlayLista PlayLista
		{
			get => _playlista;
			set
			{
				_playlista = value;
				OnPropertyChanged(nameof(PlayLista));
			}
		}


		public ICommand SaveCommand { get; }

		private void ResetFields()
		{
			PlayLista = new PlayLista();
		}

		private async void CreatePlaylista()
		{
			if (string.IsNullOrEmpty(PlayLista.naziv) || string.IsNullOrEmpty(PlayLista.putanjaSlika))
			{
				Console.WriteLine(PlayLista.naziv);
				Console.WriteLine(PlayLista.putanjaSlika);
				return;
			}

			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/PlaylistaControllerAPI", PlayLista);

				if (response.IsSuccessStatusCode)
				{
					var createdPlayLista = await response.Content.ReadFromJsonAsync<PlayLista>();
					ResetFields();
					Application.Current.MainPage.DisplayAlert("Uspješno kreirana playlista", "Playlista je uspješno kreiran", "OK");
				}
				else
				{
					Console.WriteLine("Error: " + response.ReasonPhrase);
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja playliste", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja playlista", "OK");

			}
		}


		public async void SelectImage()
		{
			try
			{
				var response4 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});


				if (response4 != null)
				{
					PlayLista.putanjaSlika = response4.FullPath;
					OnPropertyChanged(nameof(PlayLista.putanjaSlika));
					OnPropertyChanged(nameof(PlayLista));
					Application.Current.MainPage.DisplayAlert("Uspješno odabrana slika", "Slika je uspješno odabrana", "OK");

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira slike", "OK");

			}
		}

		public async void UploadImage()
		{
			if (string.IsNullOrEmpty(PlayLista.putanjaSlika))
			{
				Debug.WriteLine("No image selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(PlayLista.putanjaSlika);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(PlayLista.putanjaSlika));
				PlayLista.putanjaSlika = imageUrl;

				OnPropertyChanged(nameof(PlayLista.putanjaSlika));
				OnPropertyChanged(nameof(PlayLista));

				Application.Current.MainPage.DisplayAlert("Uspješno uploadana slika", "Slika je uspješno uploadana", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada slike", "OK");
			}
		}

		public async void SelectGif()
		{
			try
			{
				var result3 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Odaberite GIF albuma",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
				{
					{ DevicePlatform.iOS, new[] { "com.compuserve.gif" } },
					{ DevicePlatform.Android, new[] { "image/gif" } },
					{ DevicePlatform.WinUI, new[] { ".gif" } }
				})
				});
				if (result3 != null)
				{
					PlayLista.putanjaGif = result3.FullPath;
					OnPropertyChanged(nameof(PlayLista.putanjaGif));
					OnPropertyChanged(nameof(PlayLista));
					Application.Current.MainPage.DisplayAlert("Uspješno odabran GIF", "GIF je uspješno odabran", "OK");
				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira GIF-a", "OK");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira GIF-a", "OK");

			}
		}

		public async void UploadGif()
		{
			if (string.IsNullOrEmpty(PlayLista.putanjaGif))
			{
				Debug.WriteLine("No gif selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(PlayLista.putanjaGif);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(PlayLista.putanjaGif));
				PlayLista.putanjaGif = imageUrl;

				OnPropertyChanged(nameof(PlayLista.putanjaGif));
				OnPropertyChanged(nameof(PlayLista));
				Application.Current.MainPage.DisplayAlert("Uspješno uploadan GIF", "GIF je uspješno uploadan", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada GIF-a", "OK");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
