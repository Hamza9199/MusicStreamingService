using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class PjesmaPlayLista
	{
		public Int64 pjesmaID { get; set; }

		public Int64 playlistaID { get; set; }

		public Int64? pokazivacNaSljedecuPjesmuID { get; set; }

		public Int64? pokazivacNaPrethodnuPjesmuID { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }



		public PjesmaPlayLista() { }
	}
}
