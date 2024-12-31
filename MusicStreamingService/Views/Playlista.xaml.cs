using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Playlista : ContentPage
{
	public Playlista()
	{
		InitializeComponent();
		BindingContext = new PlaylistaViewModel();

	}
}