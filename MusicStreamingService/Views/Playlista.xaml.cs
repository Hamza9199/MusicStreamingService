using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Playlista : ContentPage
{
	public Playlista(Models.PlayLista odabranaPlaylista)
	{
		InitializeComponent();
		BindingContext = new PlaylistaViewModel(odabranaPlaylista);

	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var viewModel = BindingContext as PlaylistaViewModel;
		if (viewModel != null)
		{
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
		}
	}

	
}