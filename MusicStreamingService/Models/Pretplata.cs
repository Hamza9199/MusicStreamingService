using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Pretplata
	{
		public Int64 ID { get; set; }

		public string naziv { get; set; }

		public string opis { get; set; }

		public decimal cijena { get; set; }

		public bool dostupno { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public Pretplata() { }
	}
}
