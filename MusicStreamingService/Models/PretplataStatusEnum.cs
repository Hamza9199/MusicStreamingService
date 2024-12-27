using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public enum PretplataStatusEnum
	{
		[Display(Name = "Aktivna")]
		Aktivna,
		[Display(Name = "Neaktivna")]
		Neaktivna,
		[Display(Name = "Pauzirana")]
		Pauzirana,
		[Display(Name = "Istekla")]
		Istekla
	}
}
