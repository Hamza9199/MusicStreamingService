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
	public class PregledIzvodacaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public Korisnik CurrentKorisnik { get; set; }

		public PregledIzvodacaViewModel(Korisnik odabraniKorisnik)
		{
			CurrentKorisnik = odabraniKorisnik;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Songs = new ObservableCollection<Pjesma>();
			Albums = new ObservableCollection<Album>();
			Playlists = new ObservableCollection<PlayLista>();
			korisnickoIme = "Hamza";
			putanjaProfilneSlike = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUWPPJeKqMFiZdty1MgpNIUzPE0NYsz0Y0NA&s";

			LoadSongsAsync();
			LoadAlbumsAsync();
			LoadPlaylistsAsync();
		}

		private string _korisnickoIme;
		public string korisnickoIme
		{
			get => _korisnickoIme;
			set
			{
				_korisnickoIme = value;
				OnPropertyChanged(nameof(korisnickoIme));
			}
		}

		private string _putanjaProfilneSlike;
		public string putanjaProfilneSlike
		{
			get => _putanjaProfilneSlike;
			set
			{
				_putanjaProfilneSlike = value;
				OnPropertyChanged(nameof(putanjaProfilneSlike));
			}
		}

		public ObservableCollection<Pjesma> Songs { get; set; }
		public ObservableCollection<Album> Albums { get; set; }
		public ObservableCollection<PlayLista> Playlists { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private async Task LoadSongsAsync()
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
					Songs.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						Songs.Add(pjesma);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private async Task LoadAlbumsAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/AlbumControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var albumi = JsonSerializer.Deserialize<List<Album>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (albumi != null)
				{
					Albums.Clear();

					foreach (var album in albumi)
					{
						Albums.Add(album);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja albuma: {ex.Message}");
			}
		}

		private async Task LoadPlaylistsAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PlaylistaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var playliste = JsonSerializer.Deserialize<List<PlayLista>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (playliste != null)
				{
					Playlists.Clear();

					foreach (var playlist in playliste)
					{
						Playlists.Add(playlist);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja plejlista: {ex.Message}");
			}
		}
	}
}