using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;
using MusicStreamingService.Views;

namespace MusicStreamingService;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
	}
	private void OnLogoutClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
		
	}

	private void OnLajkovanePjesmeClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new LajkovanePjesme());
	}

	private void ImageButton_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new LajkovanePjesme());
	}
}
