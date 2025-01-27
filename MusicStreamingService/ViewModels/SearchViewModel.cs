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
	public class SearchViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;


		public ObservableCollection<Pjesma> Songs { get; set; }

		public ICommand SearchCommand { get; }
		public ICommand SelectSongCommand { get; }


		private bool isLoading;

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

		public event PropertyChangedEventHandler? PropertyChanged;

		public bool IsLoading
		{
			get => isLoading;
			set
			{
				isLoading = value;
				OnPropertyChanged(nameof(IsLoading));
			}
		}


		public SearchViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial")));


			Songs = new ObservableCollection<Pjesma>();

			SearchCommand = new Command<string>(OnSearch);
			SelectSongCommand = new Command(OnSongSelected);

			LoadSongsAsync();

		}

		private async void OnSongSelected()
		{

			try
			{
				if (CurrentSong != null)
				{
					await Application.Current.MainPage.Navigation.PushAsync(new AudioPlayer(CurrentSong));

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
				isLoading = true;
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
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private void OnSearch(string query)
		{

			if (string.IsNullOrWhiteSpace(query))
			{

				return;
					
			}

			try
			{
				isLoading = true;
				var filteredSongs = Songs.Where(song =>
				song.naziv.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				(song.opis != null && song.opis.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();

				Songs.Clear();
				foreach (var song in filteredSongs)
				{
					Songs.Add(song);
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

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
