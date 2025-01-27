using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class BibliotekaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;
		public ObservableCollection<Models.PlayLista> Playliste { get; set; }
		public ObservableCollection<DobiveniKorisnik> Korisnici { get; set; }

		public ICommand OnPlaylista { get; }

		public ICommand OnKorisnik { get; }

		public ICommand Otprati { get; }

		public PlayLista _currentPlaylista;
		public PlayLista CurrentPlaylista
		{
			get => _currentPlaylista;
			set
			{
				_currentPlaylista = value;
				OnPropertyChanged();
			}
		}

		public DobiveniKorisnik _currentKorisnik;

		public DobiveniKorisnik CurrentKorisnik
		{
			get => _currentKorisnik;
			set
			{
				_currentKorisnik = value;
				OnPropertyChanged();
			}
		}

		public DobiveniKorisnik CurrentKorisnik2 { get; set; }

		private bool _showPlaylists = true;
		private bool _showArtists = true;

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

		public bool ShowPlaylists
		{
			get => _showPlaylists;
			set
			{
				if (_showPlaylists != value)
				{
					_showPlaylists = value;
					OnPropertyChanged();
				}
			}
		}

		public bool ShowArtists
		{
			get => _showArtists;
			set
			{
				if (_showArtists != value)
				{
					_showArtists = value;
					OnPropertyChanged();
				}
			}
		}

		public ICommand SwitchToDefaultCommand => new Command(() =>
		{
			ShowPlaylists = true;
			ShowArtists = true;
		});

		public ICommand SwitchToPlaylistsCommand => new Command(() =>
		{
			ShowPlaylists = true;
			ShowArtists = false;
		});

		public ICommand SwitchToArtistsCommand => new Command(() =>
		{
			ShowPlaylists = false;
			ShowArtists = true;
		});


		public BibliotekaViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial")));


			Playliste = new ObservableCollection<PlayLista>();
			Korisnici = new ObservableCollection<DobiveniKorisnik>();
			OnPlaylista = new Command(onPlaylista);
			OnKorisnik = new Command(onKorisnik);
			CurrentKorisnik = new DobiveniKorisnik();
			CurrentKorisnik2 = new DobiveniKorisnik();
			CurrentPlaylista = new PlayLista();
			Otprati = new Command(OnOtprati);

			OnPropertyChanged(nameof(CurrentKorisnik));
			OnPropertyChanged(nameof(CurrentKorisnik2));

			LoadPlaylisteasync();
			LoadKorisniciAsync();
			LoadTokenData();
		}

		private async void OnOtprati(object param)
		{
			//await App.Current.MainPage.DisplayAlert("Obavijest", "Korisnik otpraćen PratilacKorisnikControllerAPI/{korisnikID/pratilacID", "OK");
			var CurrentKorisnikk = param as DobiveniKorisnik;

			if (CurrentKorisnikk == null)
			{
				return;
			}

			try
			{
				Debug.WriteLine($"api/PratilacKorisnikControllerAPI/{CurrentKorisnikk.Id}/{CurrentKorisnik2.Id}");
				var response = await _httpClient.DeleteAsync($"api/PratilacKorisnikControllerAPI/{CurrentKorisnikk.Id}/{CurrentKorisnik2.Id}");

				if (response.IsSuccessStatusCode)
				{
					await App.Current.MainPage.DisplayAlert("Obavijest", "Korisnik otpraćen", "OK");
				}
				else
				{
					await App.Current.MainPage.DisplayAlert("Greška", "Korisnik nije otpraćen", "OK");
				}
			}
			catch
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Korisnik nije otpraćen", "OK");
			}
			

			await LoadKorisniciAsync();
		}

		private async void LoadTokenData()
		{
			try
			{
				var tokenJson = await SecureStorage.GetAsync("token");
				if (!string.IsNullOrEmpty(tokenJson))
				{
					var token = System.Text.Json.JsonSerializer.Deserialize<DobiveniKorisnik>(tokenJson, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});

					if (token != null)
					{
						foreach (var claim in token.GetType().GetProperties())
						{
							Debug.WriteLine($"Claim: {claim.Name} = {claim.GetValue(token)}");
						}

						CurrentKorisnik2.Id = token.Id;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private void onKorisnik()
		{
			if (CurrentKorisnik == null) return;
			Application.Current.MainPage.Navigation.PushAsync(new PregledIzvodaca(CurrentKorisnik));
		}

		private void onPlaylista()
		{
			if (CurrentPlaylista == null) return;
			Application.Current.MainPage.Navigation.PushAsync(new Playlista(CurrentPlaylista));
		}

		private async Task LoadKorisniciAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/KorisnikControllerAPI");
				response.EnsureSuccessStatusCode();

				var response2 = await _httpClient.GetAsync($"api/PratilacKorisnikControllerAPI");
				response2.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var json2 = await response2.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json2}");

				var korisnici = System.Text.Json.JsonSerializer.Deserialize<List<DobiveniKorisnik>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				var pratilacKorisnici = System.Text.Json.JsonSerializer.Deserialize<List<PratilacKorisnik>>(json2, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				LoadTokenData();
				Debug.WriteLine(CurrentKorisnik2.Id);

				if (korisnici != null)
				{
					Korisnici.Clear();
					var filtriraniKorisnici = new List<DobiveniKorisnik>();

					foreach (var korisnik in korisnici)
					{
						var zapraceniIzvodac = pratilacKorisnici.FirstOrDefault(kp => kp.pratilacID == CurrentKorisnik2.Id && kp.korisnikID == korisnik.Id);
						if (zapraceniIzvodac != null)
						{
							filtriraniKorisnici.Add(korisnik);
						}
					}

					foreach (var korisnik in filtriraniKorisnici)
					{
						Korisnici.Add(korisnik);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja korisnika: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async Task LoadPlaylisteasync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PlaylistaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var playliste = System.Text.Json.JsonSerializer.Deserialize<List<PlayLista>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (playliste != null)
				{
					Playliste.Clear();

					foreach (var playlista in playliste)
					{
						Playliste.Add(playlista);

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

		public ICommand OnLajkovanePjesme => new Command(async () =>
		{
			await Application.Current.MainPage.Navigation.PushAsync(new LajkovanePjesme());
		});


		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
