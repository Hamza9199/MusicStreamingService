using MediaManager;
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
		public ObservableCollection<Pjesma> LajkovanePjesme { get; set; }
		public ICommand PlayPauseCommand { get; }

		private bool isPlaying;

		private Pjesma _currentSong;

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
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			LajkovanePjesme = new ObservableCollection<Pjesma>();
			PlayPauseCommand = new Command<Pjesma>(OnPlayPause);

			LoadSongsAsync();
			LoadLajkovanePjesme();

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

		private async Task LoadLajkovanePjesme()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/KorisnikPjesmaControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var korisnikPjesme = System.Text.Json.JsonSerializer.Deserialize<List<KorisnikPjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (korisnikPjesme != null)
				{
					LajkovanePjesme.Clear();
					foreach (var korisnikPjesma in korisnikPjesme)
					{
						Debug.WriteLine($"Korisnik: {korisnikPjesma.korisnikID}, Pjesma: {korisnikPjesma.pjesmaID}");
						var pjesma = Pjesme.FirstOrDefault(p => p.id == korisnikPjesma.pjesmaID);
						if (pjesma != null)
						{
							LajkovanePjesme.Add(pjesma);
						}
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private async Task LoadSongsAsync()
		{
			try
			{
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
						Pjesme.Add(pjesma);
					}

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
