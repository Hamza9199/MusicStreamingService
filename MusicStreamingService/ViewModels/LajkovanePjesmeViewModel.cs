﻿using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Views;
using Newtonsoft.Json;
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
   public class LajkovanePjesmeViewModel
	{
		private readonly HttpClient _httpClient;
		public ObservableCollection<Pjesma> Pjesme { get; set; }

		public int counter = 0;

		public string Counter { get; set; }
		public ObservableCollection<Pjesma> LajkovanePjesme { get; set; }
		public ICommand PlayPauseCommand { get; }

		public ICommand ObrisiLajk { get; }

		public DobiveniKorisnik Korisnik { get; set; }

		private bool isPlaying;

		private Pjesma _currentSong;

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

		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged();
			}
		}

		public LajkovanePjesmeViewModel()
		{
			Korisnik = new DobiveniKorisnik();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			LajkovanePjesme = new ObservableCollection<Pjesma>();
			PlayPauseCommand = new Command<Pjesma>(OnPlayPause);
			ObrisiLajk = new Command(OnObrisiLajk);

			LoadSongsAsync();
			LoadLajkovanePjesme();
			LoadTokenData();

		}

		private async void OnObrisiLajk(object param)
		{
			//await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma obrisana iz lajkovanih pjesama api/KorisnikPjesmaControllerAPI/korisnikID/pjesmaID", "U redu");

			var pjesma = param as Pjesma;
			if (pjesma == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			try
			{
				Debug.WriteLine($"api/KorisnikPjesmaControllerAPI/{Korisnik.Id}/{pjesma.id}");
				var response = await _httpClient.DeleteAsync($"api/KorisnikPjesmaControllerAPI/{Korisnik.Id}/{pjesma.id}");
				if (response.IsSuccessStatusCode)
				{
					await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma obrisana iz lajkovanih pjesama", "OK");
					await LoadLajkovanePjesme();
				}
				else
				{
					await App.Current.MainPage.DisplayAlert("Greška", "Pjesma nije obrisana iz lajkovanih pjesama", "OK");
				}
			}
			catch
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Pjesma nije obrisana iz lajkovanih pjesama", "OK");
			}

		}

		private string GetAudioPath(Pjesma pjesma)
		{
			if (pjesma == null)
			{
				System.Diagnostics.Debug.WriteLine("Greška: Nije selektovana nijedna pjesma.");
				return string.Empty;
			}

			return pjesma.putanjaAudio;
		}


		private async void OnPlayPause(Pjesma pjesma)
		{
	
			CurrentSong = pjesma;
			if (CurrentSong == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			var mediaManager = CrossMediaManager.Current;
			await mediaManager.Stop();
			
			
				string audioPath = GetAudioPath(CurrentSong);
				try
				{
					System.Diagnostics.Debug.WriteLine($"Pokušaj reprodukcije: {audioPath}");

					
					await mediaManager.Play(audioPath);
					
					

					isPlaying = true;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Greška prilikom reprodukcije: {ex.Message}");
					await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
				}
			

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

						Korisnik.Id = token.Id;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}

		private async Task LoadLajkovanePjesme()
		{
			
			try
			{
				isLoading = true;
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();
				LoadTokenData();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var korisnikPjesma = await _httpClient.GetAsync($"api/KorisnikPjesmaControllerAPI");
				korisnikPjesma.EnsureSuccessStatusCode();
				var korisnikPjesmaJson = await korisnikPjesma.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {korisnikPjesmaJson}");


				var pjesme = System.Text.Json.JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				var korisnikPjesmaList = System.Text.Json.JsonSerializer.Deserialize<List<KorisnikPjesma>>(korisnikPjesmaJson, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					LajkovanePjesme.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						var povezanaPjesma = korisnikPjesmaList.FirstOrDefault(kp => kp.pjesmaID == pjesma.id && kp.korisnikID == Korisnik.Id);

						if (povezanaPjesma != null)
						{
							LajkovanePjesme.Add(pjesma);
							counter++;
						}
					}
					Counter = $"{counter} pjesama";
					OnPropertyChanged(nameof(Counter));
					OnPropertyChanged(nameof(LajkovanePjesme));
					OnPropertyChanged(nameof(counter));
				

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				Counter = $"{counter} pjesama";
				OnPropertyChanged(nameof(Counter));
				OnPropertyChanged(nameof(LajkovanePjesme));
				OnPropertyChanged(nameof(counter));
				isLoading = false;
			}
		}

		private async Task LoadSongsAsync()
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
					Pjesme.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						if(Pjesme.Count < 10)
						{						
							Pjesme.Add(pjesma);

						}
					}

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				isLoading = false;
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
