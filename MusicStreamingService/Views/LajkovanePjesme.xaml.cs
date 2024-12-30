using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class LajkovanePjesme : ContentPage
{
	public LajkovanePjesme()
	{
		InitializeComponent();
		BindingContext = new LajkovanePjesmeViewModel();

	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		var viewModel = BindingContext as LajkovanePjesmeViewModel;
		if (viewModel != null)
		{
			viewModel.CurrentSong = null;
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
		}
	}
}