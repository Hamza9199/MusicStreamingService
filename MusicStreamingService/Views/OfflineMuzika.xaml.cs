using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class OfflineMuzika : ContentPage
{
	public OfflineMuzika()
	{
		InitializeComponent();

		BindingContext = new OfflineMuzikaViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as OfflineMuzikaViewModel;
		if (viewModel != null)
		{
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
			viewModel.CurrentSong = null;
		}
	}
}