﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class PjesmaZanr
	{
		public Int64 pjesmaID { get; set; }

		public Int64 zanrID { get; set; }

		public DateTime kreiranDatumVrijeme { get; set; }

		public Pjesma Pjesma { get; set; }

		public Zanr Zanr { get; set; }
	}
}
