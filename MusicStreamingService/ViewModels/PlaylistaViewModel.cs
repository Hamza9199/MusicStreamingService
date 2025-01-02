﻿using MusicStreamingService.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicStreamingService.ViewModels
{
	public class PlaylistaViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient _httpClient;

		public PlaylistaViewModel()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Pjesme = new ObservableCollection<Pjesma>();
			naziv = "Najbolje pjesme";
			KreiraoKorisnik = "Hamza";
			putanjaSlika = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRUWPPJeKqMFiZdty1MgpNIUzPE0NYsz0Y0NA&s";
			LoadSongsAsync();
		}

		private string _putanjaSlika;
		public string putanjaSlika
		{
			get => _putanjaSlika;
			set
			{
				_putanjaSlika = value;
				OnPropertyChanged(nameof(putanjaSlika));
			}
		}

		private string _naziv;
		public string naziv
		{
			get => _naziv;
			set
			{
				_naziv = value;
				OnPropertyChanged(nameof(naziv));
			}
		}

		private string _kreiraoKorisnik;
		public string KreiraoKorisnik
		{
			get => _kreiraoKorisnik;
			set
			{
				_kreiraoKorisnik = value;
				OnPropertyChanged(nameof(KreiraoKorisnik));
			}
		}

		

		public ObservableCollection<Pjesma> Pjesme { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
				Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}
	}
}
