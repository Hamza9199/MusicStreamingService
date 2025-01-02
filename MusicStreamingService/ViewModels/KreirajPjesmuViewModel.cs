using MusicStreamingService.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using MusicStreamingService.Services;
using System.Diagnostics;

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
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
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
					}

					var stream = await file.OpenReadAsync();
					ImageLabel.Source = ImageSource.FromStream(() => stream);

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
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
				}
				else
				{
					Console.WriteLine("Error: " + response.ReasonPhrase);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
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
