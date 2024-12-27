using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class KorisnikPjesma
	{
		public string korisnikID { get; set; }

		public Int64 pjesmaID { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }




		public KorisnikPjesma() { }
	}
}
