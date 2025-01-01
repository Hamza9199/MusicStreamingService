using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Biblioteka : ContentPage
{
	public Biblioteka()
	{
		InitializeComponent();
		BindingContext = new BibliotekaViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as BibliotekaViewModel;
		if (viewModel != null)
		{
			viewModel.CurrentSong = null;
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();



		var viewModel = BindingContext as BibliotekaViewModel;
		if (viewModel != null)
		{
			viewModel.CurrentSong = null;
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
		}
	}
}