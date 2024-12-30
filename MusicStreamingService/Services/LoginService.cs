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
		public async Task<Korisnik> Login(string username, string password)
		{
			var userInfo = new List<Korisnik>();
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			var response = await _httpClient.GetAsync($"api/KorisnikControllerAPI/{username}/{password}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				userInfo = JsonConvert.DeserializeObject<List<Korisnik>>(content);
				 
				return await Task.FromResult(userInfo.FirstOrDefault());
			}
			else
			{
				return null;
			}
		}
	}
}
