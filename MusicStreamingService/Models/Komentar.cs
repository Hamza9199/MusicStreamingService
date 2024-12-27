using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Komentar
	{
		public string korisnikID { get; set; }

		public Int64 pjesmaID { get; set; }

		public string tekst { get; set; }

		public uint vrijemePjesmeSekunde { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public Korisnik Korisnik { get; set; }

		public Pjesma Pjesma { get; set; }


		public Komentar() { }
	}
}
