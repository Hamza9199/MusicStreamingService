using MusicStreamingService.Models;
using MusicStreamingService.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class ProfilViewModel : BaseViewModel
	{
		private readonly HttpClient _httpClient;

		public string ProfilnaSlika { get; set; } = ""; 
		public string Ime { get; set; } 
		public string Prezime { get; set; } 
		public string Email { get; set; }
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
		public Command UpdateProfileCommand { get; set; }

		public ObservableCollection<Pjesma> MojePjesme { get; set; }
		public ObservableCollection<Models.Album> MojiAlbumi { get; set; }
		public ObservableCollection<Models.PlayLista> MojePlayliste { get; set; }

		public ICommand SelectSongCommand { get; }
		public ICommand SelectAlbumCommand { get; }
		public ICommand SelectPlaylistaCommand { get; }

		private Pjesma _currentSong;
		private Models.Album _currentAlbum;
		private Models.PlayLista _currentPlaylista;

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged();
			}
		}

		public Models.Album CurrentAlbum
		{
			get => _currentAlbum;
			set
			{
				_currentAlbum = value;
				OnPropertyChanged();
			}
		}

		public Models.PlayLista CurrentPlaylista
		{
			get => _currentPlaylista;
			set
			{
				_currentPlaylista = value;
				OnPropertyChanged();
			}
		}

		public ProfilViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial")));

			MojePjesme = new ObservableCollection<Pjesma>();
			MojiAlbumi = new ObservableCollection<Models.Album>();
			MojePlayliste = new ObservableCollection<Models.PlayLista>();

			UpdateProfileCommand = new Command(UpdateProfile);
			SelectSongCommand = new Command(OnSongSelected);
			SelectAlbumCommand = new Command(OnAlbumSelected);
			SelectPlaylistaCommand = new Command(OnPlaylistaSelected);

			LoadMojePjesme();
			LoadMojiAlbumi();
			LoadMojePlayliste();
			LoadTokenData();
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

						Ime = token.Ime;
						Prezime = token.Prezime;
						Email = token.AspNetUser?.Email;

						OnPropertyChanged(nameof(Ime));
						OnPropertyChanged(nameof(Prezime));
						OnPropertyChanged(nameof(Email));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private async void OnSongSelected()
		{


			try
			{
				if (CurrentSong != null)
				{
					await Application.Current.MainPage.Navigation.PushAsync(new UrediPjesmu(CurrentSong));

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}

		}

		private async void OnAlbumSelected()
		{
			try
			{
				if (CurrentAlbum != null)
				{
					await Application.Current.MainPage.Navigation.PushAsync(new UrediAlbum(CurrentAlbum));
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private async void OnPlaylistaSelected()
		{
			try
			{
				if (CurrentPlaylista != null)
				{
					await Application.Current.MainPage.Navigation.PushAsync(new UrediPlaylistu(CurrentPlaylista));
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private async void LoadMojePjesme()
		{
			try
			{
				isLoading = true;
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var pjesme = System.Text.Json.JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					MojePjesme.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						MojePjesme.Add(pjesma);
					}

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async void LoadMojiAlbumi()
		{
			try
			{
				isLoading = true;

				var response = await _httpClient.GetAsync("api/AlbumControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var albumi = System.Text.Json.JsonSerializer.Deserialize<List<Models.Album>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (albumi != null)
				{
					MojiAlbumi.Clear();
					foreach (var album in albumi)
					{
						Debug.WriteLine($"Naziv: {album.naziv}, Opis: {album.opis}");
						MojiAlbumi.Add(album);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja albuma: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		public async void LoadMojePlayliste()
		{
			try
			{
				isLoading = true;

				var response = await _httpClient.GetAsync("api/PlaylistaControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var albumi = System.Text.Json.JsonSerializer.Deserialize<List<Models.PlayLista>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (albumi != null)
				{
					MojePlayliste.Clear();
					foreach (var album in albumi)
					{
						Debug.WriteLine($"Naziv: {album.naziv}, Opis: {album.opis}");
						MojePlayliste.Add(album);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja albuma: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private void UpdateProfile()
		{
			App.Current.MainPage.DisplayAlert("UPDAJTO SI PROFIL", "OZB", "OK");
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
