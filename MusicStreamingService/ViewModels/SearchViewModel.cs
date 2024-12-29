using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStreamingService.ViewModels
{
	public class SearchViewModel
	{
		private readonly HttpClient _httpClient;


		public ObservableCollection<Pjesma> Songs { get; set; }

		public ICommand SearchCommand { get; }


		public SearchViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));


			Songs = new ObservableCollection<Pjesma>();

			SearchCommand = new Command<string>(OnSearch);

			LoadSongsAsync();

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
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private void OnSearch(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{

				return;
					
			}

			var filteredSongs = Songs.Where(song =>
				song.naziv.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				(song.opis != null && song.opis.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();

			Songs.Clear();
			foreach (var song in filteredSongs)
			{
				Songs.Add(song);
			}
		}

	}
}
