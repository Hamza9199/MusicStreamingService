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
}