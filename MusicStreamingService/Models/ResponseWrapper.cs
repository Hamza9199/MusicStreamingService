using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Models
{
	public class ResponseWrapper<T>
	{
		[Newtonsoft.Json.JsonProperty("message")]
		public string? Message { get; set; }

		[Newtonsoft.Json.JsonProperty("result")]
		public T? Result { get; set; }
	}

}
