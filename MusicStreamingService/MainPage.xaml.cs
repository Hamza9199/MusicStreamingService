using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Services;
using MusicStreamingService.ViewModels;
using MusicStreamingService.Views;

namespace MusicStreamingService;

public partial class MainPage : ContentPage
{
	private readonly ILoginRepository _loginRepository;
	public MainPage(ILoginRepository loginRepository)
	{
		InitializeComponent();
		_loginRepository = loginRepository;
		BindingContext = new MainPageViewModel();
	}


	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (await _loginRepository.isUserAuthenticated())
		{
			await Shell.Current.GoToAsync("//MainTabs");
		}
		else
		{
			await Shell.Current.GoToAsync("//Aut");

		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var viewModel = BindingContext as MainPageViewModel;
		if (viewModel != null)
		{
			viewModel.CurrentSong = null;
		}

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
