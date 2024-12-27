using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MusicStreamingService.Models
{
	public class StatistikaReprodukcije
	{
		public Int64 ID { get; set; }

		public string korisnikID { get; set; }

		public Int64 pjesmaID { get; set; }

		public int brojReprodukcija { get; set; }

		public int ukupnoTrajanje { get; set; }

		public DateTime zadnjePustanje { get; set; }


		public StatistikaReprodukcije() { }
	}
}
