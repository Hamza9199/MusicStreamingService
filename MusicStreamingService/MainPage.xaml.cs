using MusicStreamingService.ViewModels;

namespace MusicStreamingService;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel();
	}

	private void OnLogoutClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}

	
}
