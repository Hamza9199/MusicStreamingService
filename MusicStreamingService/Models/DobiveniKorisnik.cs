using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class DobiveniKorisnik
	{
		public string Id { get; set; }
		public string KorisnickoIme { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public string Bio { get; set; }
		public int StatusKorisnika { get; set; }
		public string PutanjaProfilneSlike { get; set; }
		public DateTime DatumRegistracije { get; set; }
		public DateTime? ZadnjaPrijava { get; set; }
		public int BrojPratilaca { get; set; }
		public bool Obrisan { get; set; }
		public AspNetUser AspNetUser { get; set; }
	}
}
