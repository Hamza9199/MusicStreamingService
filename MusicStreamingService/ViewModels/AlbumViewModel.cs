using MusicStreamingService.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicStreamingService.ViewModels
{
	public class AlbumViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;

		public AlbumViewModel(Models.Album odabraniAlbum)
		{

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			naziv = "Najbolje pjesme";
			Izvodjac = "Hamza";
			putanjaSlika = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUWPPJeKqMFiZdty1MgpNIUzPE0NYsz0Y0NA&s";
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

		protected virtual void OnPropertyChanged(string propertyName)
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
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
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
