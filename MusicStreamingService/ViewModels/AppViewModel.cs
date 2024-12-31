using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStreamingService.Views;

namespace MusicStreamingService.ViewModels
{
    public class AppViewModel : BaseViewModel
	{
		public bool isAdmin;

		public bool IsAdmin { get => isAdmin; set => SetProperty(ref isAdmin, value); }

		public AppViewModel()
		{
			MessagingCenter.Subscribe<Login, bool>(this, "admin", (sender, arg) =>
			{
				IsAdmin = true;
			});

			MessagingCenter.Subscribe<Login, bool>(this, "user", (sender, arg) =>
			{
				IsAdmin = false;
			});
		}
	}
}
