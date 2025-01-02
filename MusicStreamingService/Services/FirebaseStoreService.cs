using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.Services
{
	class FirebaseStoreService : IFirebaseStoreService
	{
		public async Task<string> UploadFie(Stream fileStream, string fileName)
		{
			//var cancellation = new CancellationTokenSource(); 
			var task = new FirebaseStorage("trailerflix-25df2.appspot.com", new FirebaseStorageOptions
			{
				ThrowOnCancel = true 
			})
				.Child(fileName).PutAsync(fileStream);

			try
			{
				string downloadUrl = await task;
				return downloadUrl;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error uploading: {ex.Message}");
				return null;
			}
		}
	}
}
