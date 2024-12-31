using MusicStreamingService.Models;
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

namespace MusicStreamingService.ViewModels
{
	public class ProfilViewModel : BaseViewModel
	{
		private readonly HttpClient _httpClient;

		public string ProfilnaSlika { get; set; } = "Images/dotnet_bot.png"; 
		public string Ime { get; set; } = "Hamza";
		public string Prezime { get; set; } = "Gačić";
		public string Email { get; set; } = "hamza.gacic@example.com";

		public Command UpdateProfileCommand { get; set; }

		public ObservableCollection<Pjesma> MojePjesme { get; set; }
		public ObservableCollection<Album> MojiAlbumi { get; set; }

		public ProfilViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			MojePjesme = new ObservableCollection<Pjesma>();
			MojiAlbumi = new ObservableCollection<Album>();

			UpdateProfileCommand = new Command(UpdateProfile);

			LoadMojePjesme();
			LoadMojiAlbumi();
		}

		private async void LoadMojePjesme()
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
		}

		private async void LoadMojiAlbumi()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/AlbumControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var albumi = System.Text.Json.JsonSerializer.Deserialize<List<Album>>(json, new JsonSerializerOptions
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
		}

		private void UpdateProfile()
		{

		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
