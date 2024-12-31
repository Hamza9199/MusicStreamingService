using CommunityToolkit.Maui.Views;
using MediaManager;
using MusicStreamingService.ViewModels;
using System.Diagnostics;

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

	private async void ImageButton_Clicked(object sender, EventArgs e)
	{
		/*MyBottomSheet page = new MyBottomSheet();
		page.HasHandle = true;
		await page.ShowAsync();*/
	}
}

