using MusicStreamingService.Models;
using MusicStreamingService.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class PregledIzvodacaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;

		private Pjesma _currentSong;

		public DobiveniKorisnik Korisnik { get; set; }


		private Models.Album _currentAlbum;

		private Models.PlayLista _currentPlayLista;

		public ICommand SelectSongCommand { get; }
		public ICommand SelectAlbumCommand { get; }
		public ICommand SelectPlaylistaCommand { get; }

		public ICommand Follow { get; }

		public DobiveniKorisnik CurrentKorisnik { get; set; }

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged2();
			}
		}

		public Models.Album CurrentAlbum
		{
			get => _currentAlbum;
			set
			{
				_currentAlbum = value;
				OnPropertyChanged2();
			}
		}

		public Models.PlayLista CurrentPlaylista
		{
			get => _currentPlayLista;
			set
			{
				_currentPlayLista = value;
				OnPropertyChanged2();
			}
		}

		public PregledIzvodacaViewModel(DobiveniKorisnik odabraniKorisnik)
		{
			CurrentKorisnik = odabraniKorisnik;
			Korisnik = new DobiveniKorisnik();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Songs = new ObservableCollection<Pjesma>();
			Albums = new ObservableCollection<Models.Album>();
			Playlists = new ObservableCollection<PlayLista>();
			korisnickoIme = "Hamza";
			putanjaProfilneSlike = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUWPPJeKqMFiZdty1MgpNIUzPE0NYsz0Y0NA&s";
			SelectSongCommand = new Command(OnSongSelected);
			SelectAlbumCommand = new Command(OnAlbumSelected);
			SelectPlaylistaCommand = new Command(OnPlayListaSelected);
			Follow = new Command(OnFollow);
			LoadSongsAsync();
			LoadAlbumsAsync();
			LoadPlaylistsAsync();
			LoadTokenData();
		}

		protected virtual void OnPropertyChanged2([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
		public ObservableCollection<Models.Album> Albums { get; set; }
		public ObservableCollection<PlayLista> Playlists { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		
		
		}



		private async void OnFollow()
		{
			try
			{
				LoadTokenData();
				PratilacKorisnik pratilacKorisnik = new PratilacKorisnik
				{
					pratilacID = Korisnik.Id,
					korisnikID = CurrentKorisnik.Id
				};
				Debug.WriteLine($"Pratilac: {pratilacKorisnik.pratilacID}, Korisnik: {pratilacKorisnik.korisnikID}");
				var response = await _httpClient.PostAsJsonAsync($"api/PratilacKorisnikControllerAPI", pratilacKorisnik);
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				await LoadSongsAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom praćenja izvođača: {ex.Message}");
			}
		}

		private async void OnSongSelected()
		{
			if (CurrentSong != null)
			{
				await Shell.Current.GoToAsync($"//MainTabs");
			}
		}

		private async void OnAlbumSelected()
		{
			if (CurrentAlbum != null)
			{
				await Shell.Current.GoToAsync("//MainTabs");

			}
		}

		private async void OnPlayListaSelected()
		{
			if (CurrentPlaylista != null)
			{
				await Shell.Current.GoToAsync("//MainTabs");

			}
		}

		private async void LoadTokenData()
		{
			try
			{
				var tokenJson = await SecureStorage.GetAsync("token");
				if (!string.IsNullOrEmpty(tokenJson))
				{
					var token = JsonSerializer.Deserialize<DobiveniKorisnik>(tokenJson, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});

					if (token != null)
					{
						foreach (var claim in token.GetType().GetProperties())
						{
							Debug.WriteLine($"Claim: {claim.Name} = {claim.GetValue(token)}");
						}
						Korisnik.Id = token.Id;


					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private async Task LoadSongsAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var korisnikPjesma = await _httpClient.GetAsync($"api/IzvodjacPjesmaControllerAPI");
				korisnikPjesma.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var korisnikPjesmaJson = await korisnikPjesma.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {korisnikPjesmaJson}");

				var pjesme = JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				var korisnikPjesmaList = JsonSerializer.Deserialize<List<IzvodjacPjesma>>(korisnikPjesmaJson, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});


				if (pjesme != null && korisnikPjesmaList != null)
				{
					Songs.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");

						var povezanaPjesma = korisnikPjesmaList.FirstOrDefault(kp => kp.pjesmaid == pjesma.id && kp.izvodjacid == CurrentKorisnik.Id);
						Debug.WriteLine($"Povezana pjesma: {povezanaPjesma}");
						Debug.WriteLine($"Povezana pjesma: {povezanaPjesma?.izvodjacid} - {povezanaPjesma?.pjesmaid}");
						Debug.WriteLine($"KorisnikId pjesme: {CurrentKorisnik.Id}");
						Debug.WriteLine($"PjesmaId pjesme: {pjesma.id}");
						if (povezanaPjesma != null)
						{
							Songs.Add(pjesma);
						}
							
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

				var korisnikAlbum = await _httpClient.GetAsync($"api/KorisnikAlbumControllerAPI");

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var albumi = JsonSerializer.Deserialize<List<Models.Album>>(json, new JsonSerializerOptions
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

				var korisnikPlaylista = await _httpClient.GetAsync($"api/KorisnikPlaylistaControllerAPI");

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