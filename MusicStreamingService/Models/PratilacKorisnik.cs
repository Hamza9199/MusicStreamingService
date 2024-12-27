using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class PratilacKorisnik
	{
		public string korisnikID { get; set; }

		public string pratilacID { get; set; }

		public Korisnik Korisnik { get; set; }

		public Korisnik Pratilac { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }


		public PratilacKorisnik() { }
	}
}
