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
		Task<string> Login(Korisnik korisnik);
		Task<bool> isUserAuthenticated();

		Task<string> Registracija(Korisnik korisnik);

		void LogoutAsync();
	}
}
