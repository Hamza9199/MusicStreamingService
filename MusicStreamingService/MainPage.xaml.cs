using MusicStreamingService.Models;
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


	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is MainPageViewModel viewModel && viewModel.Songs?.Any() == true)
		{
			viewModel.SelectedSong = viewModel.Songs.First();
		}
	}


}
