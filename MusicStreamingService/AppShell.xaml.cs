using MusicStreamingService.Views;

namespace MusicStreamingService
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();



		}


		
		private void AddTabBar(TabbarItem[] items)
		{
			var tabBar = new TabBar();

			foreach (var item in items)
			{
				tabBar.Items.Add(new ShellContent
				{
					Title = item.Title,
					Icon = item.Icon,
					Route = item.Route,
					ContentTemplate = new DataTemplate(item.Type)
				});
			}

			Items.Add(tabBar); 
		}

		private TabbarItem[] GetAdminTabs() => new[]
		{
			new TabbarItem("Home", "Images/dotnet_bot.png", "HomeRoute", typeof(MainPage)),
			new TabbarItem("Settings", "Images/dotnet_bot.png", "Login", typeof(Login)),
		};

		private TabbarItem[] GetUserTabs() => new[]
		{
			new TabbarItem("Home", "Images/dotnet_bot.png", "HomeRoute", typeof(MainPage)),
			new TabbarItem("Search", "Images/dotnet_bot.png", "Register", typeof(Register)),
		};

		private record TabbarItem(string Title, string Icon, string Route, Type Type);
	}
}
