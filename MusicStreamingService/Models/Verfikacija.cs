using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class Verfikacija
	{
		public String? UserId { get; set; }

		public bool? EmailConfirmed { get; set; }


		public Verfikacija() { }
	}
}
