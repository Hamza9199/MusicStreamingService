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
			var userInfo = new List<Korisnik>();
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			var response = await _httpClient.GetAsync($"Identity/Account/Login/{korisnik.korisnickoIme}/{korisnik.lozinka}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				userInfo = JsonConvert.DeserializeObject<List<Korisnik>>(content);
				if (userInfo != null)
				{
					await SecureStorage.SetAsync("token", userInfo.ToString());
					return userInfo.ToString();
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
