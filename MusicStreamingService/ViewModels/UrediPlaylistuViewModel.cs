using MusicStreamingService.Models;
using MusicStreamingService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class UrediPlaylistuViewModel : INotifyPropertyChanged
	{
		private PlayLista _playlista;
		private HttpClient _httpClient;

		public UrediPlaylistuViewModel(PlayLista odabranaPlaylista)
		{
			_playlista = odabranaPlaylista;

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(Encoding.ASCII.GetBytes("11213740:60-dayfreetrial"))
			);

			SaveCommand = new Command(UpdatePlaylista);
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

		private async void UpdatePlaylista()
		{
			if (string.IsNullOrEmpty(PlayLista.naziv) || string.IsNullOrEmpty(PlayLista.putanjaSlika))
			{
				Console.WriteLine(PlayLista.naziv);
				Console.WriteLine(PlayLista.putanjaSlika);
				return;
			}

			try
			{

				var response = await _httpClient.PutAsJsonAsync($"api/PlaylistaControllerAPI/PlayListaAPI/{PlayLista.id}", PlayLista);
				response.EnsureSuccessStatusCode();

				var updatedPlayLista = await response.Content.ReadFromJsonAsync<PlayLista>();
					ResetFields();
					Application.Current.MainPage.DisplayAlert("Uspješno uređena playlista", "Playlista je uspješno uređena", "OK");
				
				
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uređivanja plejliste", "OK");
			}
		}

		public async void SelectImage()
		{
			try
			{
				var response14 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});

				if (response14 != null)
				{
					PlayLista.putanjaSlika = response14.FullPath;
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
				var result12 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Odaberite GIF albuma",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
					{
						{ DevicePlatform.iOS, new[] { "com.compuserve.gif" } },
						{ DevicePlatform.Android, new[] { "image/gif" } },
						{ DevicePlatform.WinUI, new[] { ".gif" } }
					})
				});

				if (result12 != null)
				{
					PlayLista.putanjaGif = result12.FullPath;
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
				var gifUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(PlayLista.putanjaGif));
				PlayLista.putanjaGif = gifUrl;

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
