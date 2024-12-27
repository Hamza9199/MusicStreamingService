using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class PlayLista
	{
		public Int64 id { get; set; }

		public string korisnikID { get; set; }

		public string naziv { get; set; }

		public string? opis { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public bool javno { get; set; }

		public UInt64 brojLajkova { get; set; }

		public string? putanjaSlika { get; set; }

		public string? putanjaGif { get; set; }



		public ICollection<PjesmaPlayLista>? PjesmaPlayLista { get; set; }

		public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }


		public PlayLista() { }
	}
}
