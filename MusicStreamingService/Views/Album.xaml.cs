using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Album : ContentPage
{
	public Album()
	{
		InitializeComponent();
		BindingContext = new AlbumViewModel();

	}
}