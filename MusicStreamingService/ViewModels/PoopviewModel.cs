using MusicStreamingService.Models;
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
    public class PoopviewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;

		private Models.Pjesma _currentPlayLista;

		public DobiveniKorisnik CurrentKorisnik { get; set; }


		public Models.Album CurrentSong { get; set; }

		public Models.Pjesma CurrentPlaylista
		{
			get => _currentPlayLista;
			set
			{
				_currentPlayLista = value;
				OnPropertyChanged();
			}
		}

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

		public ObservableCollection<Models.Pjesma> Playliste { get; set; }

		public ICommand SelectPlaylistaCommand { get; }


		public PoopviewModel(Models.Album odabraniAlbum)
		{

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));
			CurrentSong = odabraniAlbum;
			CurrentKorisnik = new DobiveniKorisnik();

			Playliste = new ObservableCollection<Models.Pjesma>();

			SelectPlaylistaCommand = new Command(OnPlayListaSelected);

			LoadTokenData();
			LoadPlayListe();
		}

		private async void OnPlayListaSelected()
		{
			try
			{
				if (CurrentPlaylista != null)
				{

					Debug.WriteLine($"/api/PjesmaControllerAPI/DodajPjesmuUAlbum/{CurrentPlaylista.id}/{CurrentSong.id}");

					var response = await _httpClient.PutAsync($"api/PjesmaControllerAPI/DodajPjesmuUAlbum/{CurrentPlaylista.id}/{CurrentSong.id}", null);
					response.EnsureSuccessStatusCode();

					var responeContent = await response.Content.ReadAsStringAsync();

					Debug.WriteLine($"Response content: {responeContent}");
					Debug.WriteLine($"Response status code: {response}");
					if (response.IsSuccessStatusCode)
					{
						await Application.Current.MainPage.DisplayAlert("Pjesma dodana", $"Pjesma {CurrentPlaylista.naziv} dodana na kraj albuma {CurrentSong.naziv}", "OK");
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Greška", $"Pjesma {CurrentPlaylista.naziv} nije dodana na album {CurrentSong.naziv}", "OK");

					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Greška", $"Pjesma {CurrentPlaylista.naziv} nije dodana na album {CurrentSong.naziv}", "OK");

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
						CurrentKorisnik.Id = token.Id;


					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private async Task LoadPlayListe()
		{
			try
			{
				isLoading = true;
				LoadTokenData();
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();
				var korisnikPjesma = await _httpClient.GetAsync($"api/IzvodjacPjesmaControllerAPI");
				korisnikPjesma.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var korisnikPjesmaJson = await korisnikPjesma.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {korisnikPjesmaJson}");
				var playListe2 = System.Text.Json.JsonSerializer.Deserialize<List<Models.Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				var korisnikPjesmaList = JsonSerializer.Deserialize<List<IzvodjacPjesma>>(korisnikPjesmaJson, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (playListe2 != null)
				{
					foreach (var playLista in playListe2)
					{
						Debug.WriteLine($"Naziv: {playLista.naziv}, Opis: {playLista.opis}");
						var povezanaPjesma = korisnikPjesmaList.FirstOrDefault(kp => kp.pjesmaid == playLista.id && kp.izvodjacid == CurrentKorisnik.Id);
						Debug.WriteLine($"Povezana pjesma: {povezanaPjesma}");
						Debug.WriteLine($"Povezana pjesma: {povezanaPjesma?.izvodjacid} - {povezanaPjesma?.pjesmaid}");
						Debug.WriteLine($"KorisnikId pjesme: {CurrentKorisnik.Id}");
						Debug.WriteLine($"PjesmaId pjesme: {playLista.id}");

						if (povezanaPjesma != null)
						{
							Playliste.Add(playLista);
						}
					}


				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja play liste: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		/*try
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
			}*/

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
