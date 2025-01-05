using MusicStreamingService.Views;
using Plugin.LocalNotification;

namespace MusicStreamingService
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			MainPage = new AppShell();

			Routing.RegisterRoute("LajkovanePjesme", typeof(LajkovanePjesme));
			Routing.RegisterRoute("MainPage", typeof(MainPage));
			Routing.RegisterRoute("Register", typeof(Register));
			Routing.RegisterRoute("Login", typeof(Login));
			Routing.RegisterRoute("Search", typeof(Search));
			Routing.RegisterRoute("MainTabs/D_FAULT_Tab9/MainPage", typeof(MainPage));

			Routing.RegisterRoute("MainTabs/MainPage", typeof(MainPage));
			Routing.RegisterRoute("MainTabs/Search", typeof(Search));
		}


	}
}
