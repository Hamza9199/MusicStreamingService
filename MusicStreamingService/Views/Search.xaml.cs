using MediaManager;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Search : ContentPage
{
	public Search()
	{
		InitializeComponent();
		BindingContext = new SearchViewModel();

	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as SearchViewModel;
		if (viewModel != null)
		{
			var mediaManager = CrossMediaManager.Current;
			mediaManager.Stop();
			viewModel.CurrentSong = null;
		}
	}
}