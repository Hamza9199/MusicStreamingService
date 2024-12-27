using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{

	public enum KorisnikStatusEnum
	{
		[Display(Name = "Aktivan")]
		Aktivan,
		[Display(Name = "Neaktivan")]
		Neaktivan,
		[Display(Name = "Suspendovan")]
		Suspendovan
	}
}
