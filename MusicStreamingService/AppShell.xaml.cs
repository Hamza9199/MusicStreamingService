using MusicStreamingService.Views;

namespace MusicStreamingService
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(Login), typeof(Login));
			Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		}
	}
}
