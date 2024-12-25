using MediaManager.Library;
using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStreamingService.Models
{
	public class Korisnik
	{
		public Int64 korisnikID { get; set; }
		public string ime { get; set; }

		public string prezime { get; set; }

		public string? bio { get; set; }

		public string? korisnickoIme { get; set; }

		public string? email { get; set; }

		public string? lozinka { get; set; }

		//[EnumDataType(typeof(KorisnikStatusEnum))]
		//public KorisnikStatusEnum statusKorisnika { get; set; }

		public string? putanjaProfilneSlike { get; set; }

		public DateTime datumRegistracije { get; set; }

		public DateTime? zadnjaPrijava { get; set; }

		public UInt64 brojPratilaca { get; set; }

		public bool obrisan { get; set; }

		//public IdentityUser AspNetUser { get; set; }

		/*public ICollection<PlayLista>? PlayLista { get; set; }

		public ICollection<KorisnikPjesma>? KorisnikPjesma { get; set; }

		public ICollection<Album>? Album { get; set; }

		public ICollection<Komentar>? Komentar { get; set; }

		public ICollection<IzvodjacPjesma>? IzvodjacPjesma { get; set; }

		public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }

		public ICollection<StatistikaReprodukcije>? StatistikaReprodukcije { get; set; }*/


		public Korisnik() { }

		public Korisnik (Int64 korisnikID, string ime, string prezime, string bio, string korisnickoIme, string email, string lozinka, string putanjaProfilneSlike, DateTime datumRegistracije, DateTime? zadnjaPrijava, UInt64 brojPratilaca, bool obrisan)
		{
			this.korisnikID = korisnikID;
			this.ime = ime;
			this.prezime = prezime;
			this.bio = bio;
			this.korisnickoIme = korisnickoIme;
			this.email = email;
			this.lozinka = lozinka;
			this.putanjaProfilneSlike = putanjaProfilneSlike;
			this.datumRegistracije = datumRegistracije;
			this.zadnjaPrijava = zadnjaPrijava;
			this.brojPratilaca = brojPratilaca;
			this.obrisan = obrisan;
		}

		}
}
