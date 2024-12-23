using System;

namespace MusicStreamingService.Models
{
	public class Korisnik
	{
		public string Username { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public Korisnik() { }

		public Korisnik(string username, string email, string password)
		{
			Username = username;
			Email = email;
			Password = password;
		}

		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(Username) &&
				   !string.IsNullOrWhiteSpace(Email) &&
				   !string.IsNullOrWhiteSpace(Password);
		}
	}
}
