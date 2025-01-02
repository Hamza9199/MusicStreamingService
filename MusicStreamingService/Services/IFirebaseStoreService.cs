using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Services
{
    public interface IFirebaseStoreService
    {
		Task<string> UploadFie(Stream fileStream, string fileName);
	}
}
