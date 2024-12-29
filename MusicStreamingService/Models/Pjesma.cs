using MediaManager.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Pjesma
	{
		public Int64 id { get; set; }

		public Int64? albumID { get; set; }

		public Int64? redniBrojUAlbumu { get; set; }

		public String naziv { get; set; }

		public string opis { get; set; }

		public DateOnly datumObjave { get; set; }

		public uint trajanjeSekunde { get; set; }

		public bool javno { get; set; }

		public bool odobreno { get; set; }

		public string putanjaAudio { get; set; }

		public string? putanjaSlika { get; set; }

		public string? putanjaGif { get; set; }

		public UInt64 brojReprodukcija { get; set; }

		public UInt64 brojLajkova { get; set; }

		public string jezikPjesme { get; set; }

		public string? licenca { get; set; }

		public bool eksplicitniSadrzaj { get; set; }

		public string? tekst { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public ICollection<PjesmaPlayLista>? PjesmaPlayLista { get; set; }

		public ICollection<KorisnikPjesma>? KorisnikPjesma { get; set; }

		public ICollection<Komentar>? Komentar { get; set; }

		public ICollection<PjesmaZanr>? PjesmaZanr { get; set; }

		public ICollection<IzvodjacPjesma>? IzvodjacPjesma { get; set; }

		public ICollection<HistorijaSlusanja>? HistorijaSlusanja { get; set; }

		public ICollection<StatistikaReprodukcije>? StatistikaReprodukcije { get; set; }


		public Pjesma() { }

		/*public Pjesma(string naziv,string opis, string putanjaAudio, string putanjaSlika, string jezikPjesme, DateTime kreiranDatumVrijeme)
		{
			this.naziv = naziv;
			this.opis = opis;
			this.putanjaAudio = putanjaAudio;
			this.putanjaSlika = putanjaSlika;
			this.jezikPjesme = jezikPjesme;
			this.kreiranDatumVrijeme = kreiranDatumVrijeme;

		}*/
	}

}
