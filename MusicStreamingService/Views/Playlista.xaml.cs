using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Playlista : ContentPage
{
	public Playlista(Models.PlayLista odabranaPlaylista)
	{
		InitializeComponent();
		BindingContext = new PlaylistaViewModel(odabranaPlaylista);

	}
}