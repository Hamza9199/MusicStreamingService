using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;


namespace MusicStreamingService.Services
{
	public class LoginService : ILoginRepository
	{
		public async Task<string> Login(Korisnik korisnik)
		{
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			var content = new StringContent(JsonConvert.SerializeObject(korisnik), Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync($"api/KorisnikControllerAPI/Login", content);

			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				var userInfo = JsonConvert.DeserializeObject<List<Korisnik>>(responseContent);

				if (userInfo != null && userInfo.Any())
				{
					await SecureStorage.SetAsync("token", userInfo.FirstOrDefault()?.ToString());
					return userInfo.FirstOrDefault()?.ToString();
				}

				return null;
			}
			else
			{
				return null;
			}
		}

		public async Task<string> Registracija(Korisnik korisnik)
		{
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));
			
			var content = new StringContent(JsonConvert.SerializeObject(korisnik), Encoding.UTF8, "application/json");
			
			var response = await _httpClient.PostAsync($"api/KorisnikControllerAPI/Registracija", content);

			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				var userInfo = JsonConvert.DeserializeObject<List<Korisnik>>(responseContent);
				if (userInfo != null && userInfo.Any())
				{
					await SecureStorage.SetAsync("token", userInfo.FirstOrDefault()?.ToString());
					return userInfo.FirstOrDefault()?.ToString();
				}
				return null;
			}
			else
			{
				return null;
			}
		}


		public async Task<bool> isUserAuthenticated()
		{
			var token = await SecureStorage.GetAsync("token");

			if (token != null)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public void LogoutAsync()
		{
			SecureStorage.Default.Remove("token");
		}

	}
}
