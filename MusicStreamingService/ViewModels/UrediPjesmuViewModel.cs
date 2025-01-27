using MusicStreamingService.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using MusicStreamingService.Services;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;

namespace MusicStreamingService.ViewModels
{
	public class UrediPjesmuViewModel : INotifyPropertyChanged
	{
		private Pjesma _pjesma2;
		private Pjesma _novaPjesma;
		private HttpClient _httpClient;

		public UrediPjesmuViewModel(Pjesma odabranaPjesma)
		{
			_pjesma2 = odabranaPjesma;
			_novaPjesma = new Pjesma();
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial"))
			);
			SaveCommand = new Command(UpdatePjesmaAsync);
			PropertyChanged = delegate { };
		}

		public Pjesma Pjesma2
		{
			get => _pjesma2;
			set
			{
				_pjesma2 = value;
				OnPropertyChanged(nameof(Pjesma2));
			}
		}

		public Pjesma NovaPjesma
		{
			get => _novaPjesma;
			set
			{
				_novaPjesma = value;
				OnPropertyChanged(nameof(NovaPjesma));
			}
		}

		public ICommand SaveCommand { get; }

		public async void SelectAudio()
		{
			try
			{
				var response10 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Audio Fajl",
					FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
					{
						{ DevicePlatform.iOS, new[] { "public.audio" } },
						{ DevicePlatform.Android, new[] { "audio/*" } },
						{ DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".m4a" } }
					})
				});

				if (response10 != null)
				{
					Pjesma2.putanjaAudio = response10.FullPath;
					OnPropertyChanged(nameof(Pjesma2.putanjaAudio));
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
			if (string.IsNullOrEmpty(Pjesma2.putanjaAudio))
			{
				Debug.WriteLine("No audio file selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Pjesma2.putanjaAudio);
				var firebaseStorage = new FirebaseStoreService();
				var audioUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Pjesma2.putanjaAudio));
				Pjesma2.putanjaAudio = audioUrl;

				OnPropertyChanged(nameof(Pjesma2.putanjaAudio));
				OnPropertyChanged(nameof(Pjesma2));
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
				var response10 = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Izaberite Sliku",
					FileTypes = FilePickerFileType.Images
				});

				if (response10 != null)
				{
					Pjesma2.putanjaSlika = response10.FullPath;
					OnPropertyChanged(nameof(Pjesma2.putanjaSlika));
					OnPropertyChanged(nameof(Pjesma2));
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
			if (string.IsNullOrEmpty(Pjesma2.putanjaSlika))
			{
				Debug.WriteLine("No image selected.");
				return;
			}

			try
			{
				using var fileStream = File.OpenRead(Pjesma2.putanjaSlika);
				var firebaseStorage = new FirebaseStoreService();
				var imageUrl = await firebaseStorage.UploadFie(fileStream, Path.GetFileName(Pjesma2.putanjaSlika));
				Pjesma2.putanjaSlika = imageUrl;

				OnPropertyChanged(nameof(Pjesma2.putanjaSlika));
				OnPropertyChanged(nameof(Pjesma2));
				Application.Current.MainPage.DisplayAlert("Uspješno uploadana slika", "Slika je uspješno uploadana", "OK");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom uploada slike", "OK");
			}
		}

		private async void UpdatePjesmaAsync()
		{
			if (string.IsNullOrEmpty(Pjesma2.naziv) || string.IsNullOrEmpty(Pjesma2.putanjaAudio))
			{
				Console.WriteLine(Pjesma2.naziv);
				Console.WriteLine(Pjesma2.putanjaAudio);
				return;
			}

			try
			{
				Debug.WriteLine(Pjesma2);
				foreach (var prop in Pjesma2.GetType().GetProperties())
				{
					Debug.WriteLine(prop.Name + ": " + prop.GetValue(Pjesma2));
				}
			  
				//var jsonString = new StringContent(JsonConvert.SerializeObject(Pjesma2), Encoding.UTF8, "application/json");
				

				var response = await _httpClient.PutAsJsonAsync($"api/PjesmaControllerAPI/PutPjesmaAPI/{Pjesma2.id}", Pjesma2);
				response.EnsureSuccessStatusCode();

				Debug.WriteLine("Response: " + response.StatusCode);
				Debug.WriteLine("Response: " + response);
				string responseContent = await response.Content.ReadAsStringAsync();
				Debug.WriteLine("Response content: " + responseContent);
				
					var updatedPjesma = await response.Content.ReadFromJsonAsync<Pjesma>();
					Application.Current.MainPage.DisplayAlert("Uspješno ažurirana pjesma", "Pjesma je uspješno ažurirana", "OK");
				
				
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom ažuriranja pjesme", "OK");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			Debug.WriteLine("Property changed: " + propertyName);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
