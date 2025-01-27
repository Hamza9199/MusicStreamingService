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
	public class PoopopviewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;

		private Models.PlayLista _currentPlayLista;

		public Pjesma CurrentSong { get; set; }

		public Models.PlayLista CurrentPlaylista
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

		public ObservableCollection<Models.PlayLista> Playliste { get; set; }

		public ICommand SelectPlaylistaCommand { get; }

		public PoopopviewModel(Pjesma odabranaPjesma)
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial")));
			CurrentSong = odabranaPjesma;

			Playliste = new ObservableCollection<Models.PlayLista>();

			SelectPlaylistaCommand = new Command(OnPlayListaSelected);

			LoadPlayListe();

		}

		private async void OnPlayListaSelected()
		{
			try
			{
				if (CurrentPlaylista != null)
				{

					Debug.WriteLine($" /api/PjesmaPlayListaControllerAPI/DodajPjesmuNaKrajListe/pjesmaID/playlistaID ");

					var response = await _httpClient.PostAsync($"/api/PjesmaPlayListaControllerAPI/DodajPjesmuNaKrajListe/{CurrentSong.id}/{CurrentPlaylista.id}", null);
					response.EnsureSuccessStatusCode();

					var responeContent = await response.Content.ReadAsStringAsync();

					Debug.WriteLine($"Response content: {responeContent}");
					Debug.WriteLine($"Response status code: {response}");
					if (response.IsSuccessStatusCode)
					{
						await Application.Current.MainPage.DisplayAlert("Pjesma dodana", $"Pjesma {CurrentSong.naziv} dodana na kraj play liste {CurrentPlaylista.naziv}", "OK");
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Greška", $"Pjesma {CurrentSong.naziv} nije dodana na play listu {CurrentPlaylista.naziv}", "OK");

					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
				await Application.Current.MainPage.DisplayAlert("Greška", $"Pjesma {CurrentSong.naziv} nije dodana na play listu {CurrentPlaylista.naziv}", "OK");

			}

		}


		private async Task LoadPlayListe()
		{
			try
			{
				isLoading = true;

				var response = await _httpClient.GetAsync("api/PlaylistaControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var playListe2 = System.Text.Json.JsonSerializer.Deserialize<List<Models.PlayLista>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (playListe2 != null)
				{
					foreach (var playLista in playListe2)
					{
						Debug.WriteLine($"Naziv: {playLista.naziv}, Opis: {playLista.opis}");
						Playliste.Add(playLista);
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

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
