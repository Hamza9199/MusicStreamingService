using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Profil : ContentPage
{
	public Profil()
	{
		InitializeComponent();
		BindingContext = new ProfilViewModel();
	}


	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as ProfilViewModel;
		if (viewModel != null)
		{
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
			viewModel.CurrentSong = null;
			viewModel.CurrentAlbum = null;
			viewModel.CurrentPlaylista = null;
		}
	}
}