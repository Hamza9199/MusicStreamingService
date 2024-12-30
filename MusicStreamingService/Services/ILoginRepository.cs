using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Services
{
    public interface ILoginRepository
    {
		Task<Korisnik> Login(string username, string password);
	}
}
