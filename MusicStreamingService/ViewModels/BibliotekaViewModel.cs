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

		public PlayLista CurrentPlaylista { get; set; }

		public DobiveniKorisnik CurrentKorisnik { get; set; }

		private bool _showPlaylists = true;
		private bool _showArtists = true;

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
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));


			Playliste = new ObservableCollection<PlayLista>();
			Korisnici = new ObservableCollection<DobiveniKorisnik>();
			OnPlaylista = new Command(onPlaylista);
			OnKorisnik = new Command(onKorisnik);

			LoadPlaylisteasync();
			LoadKorisniciAsync();
		}

		private void onKorisnik()
		{
			Application.Current.MainPage.Navigation.PushAsync(new PregledIzvodaca(CurrentKorisnik));
		}

		private void onPlaylista()
		{
			Application.Current.MainPage.Navigation.PushAsync(new Playlista(CurrentPlaylista));
		}

		private async Task LoadKorisniciAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/KorisnikControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var korisnici = System.Text.Json.JsonSerializer.Deserialize<List<DobiveniKorisnik>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (korisnici != null)
				{
					Korisnici.Clear();
					foreach (var korisnik in korisnici)
					{
						Korisnici.Add(korisnik);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja korisnika: {ex.Message}");
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
