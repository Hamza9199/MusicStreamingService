using System;

namespace MusicStreamingService.Models
{
	public class VerifikovaniKorisnik
	{
		[Newtonsoft.Json.JsonProperty("id")]
		public string? Id { get; set; }

		[Newtonsoft.Json.JsonProperty("userName")]
		public string? UserName { get; set; }

		[Newtonsoft.Json.JsonProperty("normalizedUserName")]
		public string? NormalizedUserName { get; set; }

		[Newtonsoft.Json.JsonProperty("email")]
		public string? Email { get; set; }

		[Newtonsoft.Json.JsonProperty("normalizedEmail")]
		public string? NormalizedEmail { get; set; }

		[Newtonsoft.Json.JsonProperty("emailConfirmed")]
		public bool? EmailConfirmed { get; set; }

		[Newtonsoft.Json.JsonProperty("passwordHash")]
		public string? PasswordHash { get; set; }

		[Newtonsoft.Json.JsonProperty("securityStamp")]
		public string? SecurityStamp { get; set; }

		[Newtonsoft.Json.JsonProperty("concurrencyStamp")]
		public string? ConcurrencyStamp { get; set; }

		[Newtonsoft.Json.JsonProperty("phoneNumber")]
		public string? PhoneNumber { get; set; }

		[Newtonsoft.Json.JsonProperty("phoneNumberConfirmed")]
		public bool? PhoneNumberConfirmed { get; set; }

		[Newtonsoft.Json.JsonProperty("twoFactorEnabled")]
		public bool? TwoFactorEnabled { get; set; }

		[Newtonsoft.Json.JsonProperty("lockoutEnd")]
		public DateTimeOffset? LockoutEnd { get; set; }

		[Newtonsoft.Json.JsonProperty("lockoutEnabled")]
		public bool? LockoutEnabled { get; set; }

		[Newtonsoft.Json.JsonProperty("accessFailedCount")]
		public int? AccessFailedCount { get; set; }

		public VerifikovaniKorisnik() { }
	}
}
