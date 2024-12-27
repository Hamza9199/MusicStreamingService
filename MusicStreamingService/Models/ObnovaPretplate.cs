using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{

	public class ObnovaPretplate
	{
		[Key]
		public Int64 id { get; set; }

		public Int64 korisnikPretplataID { get; set; }

		public DateTime datumObnove { get; set; }

		public decimal iznosObnove { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }
	}
}
