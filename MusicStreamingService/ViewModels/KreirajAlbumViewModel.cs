using MusicStreamingService.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using MusicStreamingService.Services;
using System.Net.Http.Json;

namespace MusicStreamingService.ViewModels
{
	public class KreirajAlbumViewModel : INotifyPropertyChanged
	{
		private Album _album;
		private HttpClient _httpClient;


		public event PropertyChangedEventHandler? PropertyChanged;


		public KreirajAlbumViewModel()
		{
			_album = new Album();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial"))
			);

			SaveCommand = new Command(CreateAlbum);
			PropertyChanged = delegate { };
		}

		public Album Album
		{
			get => _album;
			set
			{
				_album = value;
				OnPropertyChanged(nameof(Album));
			}
		}


		public ICommand SaveCommand { get; }

		private void ResetFields()
		{
			Album = new Album();
		}

		private async void CreateAlbum()
		{
			if (string.IsNullOrEmpty(Album.naziv) || string.IsNullOrEmpty(Album.putanjaSlika))
			{
				Console.WriteLine(Album.naziv);
				Console.WriteLine(Album.putanjaSlika);
				return;
			}

			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/AlbumControllerAPI", Album);

				if (response.IsSuccessStatusCode)
				{
					var createdAlbum = await response.Content.ReadFromJsonAsync<Album>();
					ResetFields();
					Application.Current.MainPage.DisplayAlert("Uspješno kreiran album", "Album je uspješno kreiran", "OK");
				}
				else
				{
					Console.WriteLine("Error: " + response.ReasonPhrase);
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja albuma", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja albuma", "OK");

			}
		}

		public async void SelectImage()
		{
			try
			{
				var response3 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});

				if (response3 != null)
				{
					Album.putanjaSlika = response3.FullPath;
					OnPropertyChanged(nameof(Album.putanjaSlika));
					OnPropertyChanged(nameof(Album));
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
			if (string.IsNullOrEmpty(Album.putanjaSlika))
			{
				Debug.WriteLine("No image selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Album.putanjaSlika);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Album.putanjaSlika));
				Album.putanjaSlika = imageUrl;

				OnPropertyChanged(nameof(Album.putanjaSlika));
				OnPropertyChanged(nameof(Album));

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
				var result2 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Odaberite GIF albuma",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
				{
					{ DevicePlatform.iOS, new[] { "com.compuserve.gif" } },
					{ DevicePlatform.Android, new[] { "image/gif" } },
					{ DevicePlatform.WinUI, new[] { ".gif" } }
				})
				});
				if (result2 != null)
				{
					Album.putanjaGif = result2.FullPath;
					OnPropertyChanged(nameof(Album.putanjaGif));
					OnPropertyChanged(nameof(Album));
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
			if (string.IsNullOrEmpty(Album.putanjaGif))
			{
				Debug.WriteLine("No gif selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Album.putanjaGif);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Album.putanjaGif));
				Album.putanjaGif = imageUrl;

				OnPropertyChanged(nameof(Album.putanjaGif));
				OnPropertyChanged(nameof(Album));
				Application.Current.MainPage.DisplayAlert("Uspješno uploadan GIF", "GIF je uspješno uploadan", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada GIF-a", "OK");
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
