using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class KorisnikPretplata
	{
	
		public Int64 id { get; set; }

		public string korisnikID { get; set; }

		public Int64 pretplataID { get; set; }

		public PretplataStatusEnum PretplataStatus { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public DateTime datumVrijemeObnove { get; set; }

		public DateTime datumIsteka { get; set; }

		public KorisnikPretplata() { }

	}
}
