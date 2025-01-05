using MusicStreamingService.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using MusicStreamingService.Services;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.Json;

namespace MusicStreamingService.ViewModels
{
	public class KreirajPjesmuViewModel : INotifyPropertyChanged
	{
		private Pjesma _pjesma;
		private HttpClient _httpClient;

		public KreirajPjesmuViewModel()
		{
			_pjesma = new Pjesma();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial"))
			);

			SaveCommand = new Command(SavePjesmaAsync);
			PropertyChanged = delegate { };
		}

		public Pjesma Pjesma
		{
			get => _pjesma;
			set
			{
				_pjesma = value;
				OnPropertyChanged(nameof(Pjesma));
			}
		}

		public Image ImageLabel { get; set; }

		public ICommand SaveCommand { get; }



		public async void SelectAudio()
		{
			try
			{
				var response1 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Audio Fajl",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
					{
						{ DevicePlatform.iOS, new[] { "public.audio" } },
						{ DevicePlatform.Android, new[] { "audio/*" } },
						{ DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a" } }
					})
				});

				if (response1 != null)
				{
					Pjesma.putanjaAudio = response1.FullPath;
					OnPropertyChanged(nameof(Pjesma.putanjaAudio));
					OnPropertyChanged(nameof(Pjesma));
					Application.Current.MainPage.DisplayAlert("Uspješno odabran audio fajl", "Audio fajl je uspješno odabran", "OK");
				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira audio fajla", "OK");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira audio fajla", "OK");
			}
		}

		public async void UploadAudio()
		{
			if (string.IsNullOrEmpty(Pjesma.putanjaAudio))
			{
				Debug.WriteLine("No audio file selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Pjesma.putanjaAudio);
				var firebaseStorage = new FirebaseStoreService();
				var audioUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Pjesma.putanjaAudio));
				Pjesma.putanjaAudio = audioUrl;

				OnPropertyChanged(nameof(Pjesma.putanjaAudio));
				OnPropertyChanged(nameof(Pjesma));
				Application.Current.MainPage.DisplayAlert("Uspješno uploadan audio fajl", "Audio fajl je uspješno uploadan", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada audio fajla", "OK");

			}
		}

		public async void SelectImage()
		{
			try
			{
				var response2 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});

				if (response2 != null)
				{
					Pjesma.putanjaSlika = response2.FullPath;
					OnPropertyChanged(nameof(Pjesma.putanjaSlika));
					OnPropertyChanged(nameof(Pjesma));

					Application.Current.MainPage.DisplayAlert("Uspješno odabrana slika", "Slika je uspješno odabrana", "OK");

				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira slike", "OK");
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
			if (string.IsNullOrEmpty(Pjesma.putanjaSlika))
			{
				Debug.WriteLine("No image selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Pjesma.putanjaSlika);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Pjesma.putanjaSlika));
				Pjesma.putanjaSlika = imageUrl;

				OnPropertyChanged(nameof(Pjesma.putanjaSlika));
				OnPropertyChanged(nameof(Pjesma));
				Application.Current.MainPage.DisplayAlert("Uspješno uploadana slika", "Slika je uspješno uploadana", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada slike", "OK");
			}
		}


		public async void AudioUpload()
		{
			try
			{
				var response = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Audio Fajl",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
					{
						{ DevicePlatform.iOS, new[] { "public.audio" } },
						{ DevicePlatform.Android, new[] { "audio/*" } },
						{ DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a" } }
					})
				});

				if (response != null)
				{
					var file = response;
					Pjesma.putanjaAudio = file.FullPath;

					using (var fileStream = await file.OpenReadAsync())
					{
						var firebaseStorage = new FirebaseStoreService();
						var audioUrl = await firebaseStorage.UploadFie(fileStream, file.FileName);
						Pjesma.putanjaAudio = audioUrl;

						OnPropertyChanged(nameof(Pjesma.putanjaAudio));
						Application.Current.MainPage.DisplayAlert("Uspješno odabran audio fajl", "Audio fajl je uspješno odabran", "OK");
					}
				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira audio fajla", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira audio fajla", "OK");
			}
		}

		public async void ImageUpload()
		{
			try
			{
				var response = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});

				if (response != null)
				{
					var file = response;
					Pjesma.putanjaSlika = file.FullPath;

					using (var fileStream = await file.OpenReadAsync())
					{
						var firebaseStorage = new FirebaseStoreService();
						var imageUrl = await firebaseStorage.UploadFie(fileStream, file.FileName);
						Debug.WriteLine(imageUrl);
						Pjesma.putanjaSlika = imageUrl;

						OnPropertyChanged(nameof(Pjesma.putanjaSlika));
						Application.Current.MainPage.DisplayAlert("Uspješno uploadana slika", "Slika je uspješno uploadana", "OK");
					}

					var stream = await file.OpenReadAsync();
					ImageLabel.Source = ImageSource.FromStream(() => stream);

				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira slike", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom odabira slike", "OK");
			}
		}

		private async void SavePjesmaAsync()
		{
			if (string.IsNullOrEmpty(Pjesma.naziv) || string.IsNullOrEmpty(Pjesma.putanjaAudio))
			{
				Console.WriteLine(Pjesma.naziv);
				Console.WriteLine(Pjesma.putanjaAudio);
				return;
			}

			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/PjesmaControllerAPI", Pjesma);

				if (response.IsSuccessStatusCode)
				{
					var createdPjesma = await response.Content.ReadFromJsonAsync<Pjesma>();
					ResetFields();
					Application.Current.MainPage.DisplayAlert("Uspješno kreirana pjesma", "Pjesma je uspješno kreirana", "OK");
				}
				else
				{
					Console.WriteLine("Error: " + response.ReasonPhrase);
					Debug.WriteLine(response);
					var responseContent = await response.Content.ReadAsStringAsync();
					Debug.WriteLine(responseContent);
					Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja pjesme", "OK");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom kreiranja pjesme", "OK");
			}
		}

		private void ResetFields()
		{
			Pjesma = new Pjesma();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
