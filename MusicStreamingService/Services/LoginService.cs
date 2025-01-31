﻿using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;


namespace MusicStreamingService.Services
{
	public class LoginService : ILoginRepository
	{
		public async Task<string> Login(Korisnik korisnik)
		{
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial"))
			);

			var jsonPayload = JsonConvert.SerializeObject(korisnik);
			Debug.WriteLine("JSON Payload: " + jsonPayload);

			var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("api/KorisnikControllerAPI/Login", content);
			Debug.WriteLine($"Response: {response.StatusCode}");

			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Debug.WriteLine("Response Content: " + responseContent);

				//var userInfo = JsonConvert.DeserializeObject<List<Korisnik>>(responseContent);

				if (!string.IsNullOrWhiteSpace(responseContent))
				{
					var encodedEmail = Uri.EscapeDataString(korisnik.email);
					Debug.WriteLine("Encoded Email: " + encodedEmail);
					var response2 = await _httpClient.GetAsync("api/KorisnikControllerAPI/Email/" + encodedEmail);

					string responseContent2 = await response2.Content.ReadAsStringAsync();
					Debug.WriteLine("Response Content: " + responseContent2);

					var korisnikResponse = JsonConvert.DeserializeObject<Dictionary<string, DobiveniKorisnik>>(responseContent2);

					var citavKorisnik = korisnikResponse;
					Debug.WriteLine("Citav Korisnik: " + citavKorisnik);
					foreach (var item in citavKorisnik)
					{
						Debug.WriteLine("Item: " + item);
					}
					await SecureStorage.SetAsync("citavKorisnik", JsonConvert.SerializeObject(citavKorisnik));

					DobiveniKorisnik korisnik2 = korisnikResponse["korisnik"];
					//AspNetUser aspKorisnik = korisnikResponse[""]
					Debug.WriteLine("Korisnik2: " + korisnik2);

					var Korisnik = korisnikResponse["korisnik"];
					Debug.WriteLine("Korisnik: " + Korisnik);
					
					for(int i = 0; i < korisnikResponse.Count; i++)
					{
						Debug.WriteLine("Korisnik: " + korisnikResponse.ElementAt(i));
					}

					await SecureStorage.SetAsync("korisnik", Korisnik.ToString());

					var token = JsonConvert.SerializeObject(korisnik2);
					Debug.WriteLine("Token: " + token);
					await SecureStorage.SetAsync("token", token);
					return null;
				}

				return "Unexpected error: No response content.";
			}
			else
			{
				var errorContent = await response.Content.ReadAsStringAsync();
				Debug.WriteLine("Error Content: " + errorContent);
				return $"Login failed: {errorContent}";
			}
		}


		public async Task<string> Registracija(Korisnik korisnik)
		{
			var _httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibel-001-site1.anytempurl.com/")
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
				"Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11213740:60-dayfreetrial"))
			);

			var content = new StringContent(JsonConvert.SerializeObject(korisnik), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync($"api/KorisnikControllerAPI/Registracija", content);

			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();

				var responseWrapper = JsonConvert.DeserializeObject<ResponseWrapper<VerifikovaniKorisnik>>(responseContent);

				if (responseWrapper?.Result == null)
				{
					return "Error: Failed to retrieve registered user information.";
				}

				VerifikovaniKorisnik registeredUser = responseWrapper.Result;

				Debug.WriteLine("Registered User: " + responseWrapper.Result);
				Debug.WriteLine("Registered User: " + responseWrapper);
				Debug.WriteLine("Registered User: " + responseContent);
				Debug.WriteLine("Registered User: " + registeredUser.ToString());

				Verfikacija verifikacija = new Verfikacija
				{
					UserId = registeredUser.Id,
					EmailConfirmed = true
				};
				Debug.WriteLine(registeredUser.Id);
				Debug.WriteLine(verifikacija.UserId);

				var verifikacijaContent = new StringContent(JsonConvert.SerializeObject(verifikacija), Encoding.UTF8, "application/json");
				Debug.WriteLine(verifikacijaContent);
				var verifikacijaResponse = await _httpClient.PutAsync($"api/KorisnikControllerAPI/UpdateEmailConfirmed", verifikacijaContent);
				Debug.WriteLine(verifikacijaResponse);

				if (verifikacijaResponse.IsSuccessStatusCode)
				{
					return null;
				}
				else
				{
					string errorContent = await verifikacijaResponse.Content.ReadAsStringAsync();
					return $"Error in verification step: {errorContent}";
				}
			}
			else
			{
				string errorContent = await response.Content.ReadAsStringAsync();
				return $"Error during registration: {errorContent}";
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
			SecureStorage.Default.Remove("citavKorisnik");
			SecureStorage.Default.Remove("korisnik");

		}

	}
}
