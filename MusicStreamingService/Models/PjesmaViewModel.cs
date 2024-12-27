using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class PjesmaViewModel
	{
		public string naziv { get; set; }
		public string opis { get; set; }
		public string putanjaSlika { get; set; }
		public string jezikPjesme { get; set; }
		public DateTime kreiranDatumVrijeme { get; set; }
	}

}
