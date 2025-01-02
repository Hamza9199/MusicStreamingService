using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class KreirajAlbum : ContentPage
{
	public KreirajAlbum()
	{
		InitializeComponent();
		BindingContext = new KreirajAlbumViewModel();

	}
}