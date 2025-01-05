using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicStreamingService.ViewModels
{
	public class DetaljiRacunaViewModel : INotifyPropertyChanged
	{
		private string _ime;
		private string _korisnickoIme;


		public string Ime
		{
			get => _ime;
			set
			{
				_ime = value;
				OnPropertyChanged(nameof(Ime));
			}
		}

		public string KorisnickoIme
		{
			get => _korisnickoIme;
			set
			{
				_korisnickoIme = value;
				OnPropertyChanged(nameof(KorisnickoIme));
			}
		}


		public DetaljiRacunaViewModel()
		{

			LoadTokenData();

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

						Ime = token.Ime;
						KorisnickoIme = token.KorisnickoIme;


						OnPropertyChanged(nameof(Ime));

						OnPropertyChanged(nameof(KorisnickoIme));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
			}
		}


		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
