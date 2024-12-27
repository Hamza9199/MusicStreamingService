using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class HistorijaSlusanja
	{
		public Int64 id { get; set; }

		public string korisnikID { get; set; }

		public Int64 pjesmaID { get; set; }

		public Int64? playlistaID { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public int trajanje { get; set; }

		public KontekstPustanja kontekstPustanja { get; set; }

		public bool offline { get; set; }


		public HistorijaSlusanja() { }
	}
}
