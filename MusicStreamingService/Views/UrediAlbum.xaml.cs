using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class UrediAlbum : ContentPage
{
	public UrediAlbum()
	{
		InitializeComponent();
		BindingContext = new UrediAlbumViewModel();

	}
}