using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Album : ContentPage
{
	public Album(Models.Album odabraniAlbum)
	{
		InitializeComponent();
		BindingContext = new AlbumViewModel(odabraniAlbum);

	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var viewModel = BindingContext as AlbumViewModel;
		if (viewModel != null)
		{
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
		}
	}

	
}