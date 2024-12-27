using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class IzvodjacPjesma
	{
		public string izvodjacid { get; set; }

		public Int64 pjesmaid { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }



		public IzvodjacPjesma() { }
	}
}
