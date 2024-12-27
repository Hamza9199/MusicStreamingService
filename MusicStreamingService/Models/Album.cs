using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Album
	{
		public Int64 id { get; set; }

		public string korisnikid { get; set; }

		public string naziv { get; set; }

		public string? opis { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public bool odobreno { get; set; }

		public bool javno { get; set; }

		public UInt64 brojLajkova { get; set; }

		public string? putanjaSlika { get; set; }

		public string? putanjaGif { get; set; }

		public ICollection<Pjesma> Pjesma { get; set; }

		


		public Album() { }

	}
}
