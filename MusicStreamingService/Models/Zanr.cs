using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Zanr
	{
		public Int64 ID { get; set; }

		public String naziv { get; set; }

		public String opis { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public ICollection<PjesmaZanr>? PjesmaZanr { get; set; }

	}
}
