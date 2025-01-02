namespace MusicStreamingService.Views;

using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;
using Newtonsoft.Json;
using System.Text.Json;

public partial class AudioPlayer : ContentPage
{
	public AudioPlayer(Pjesma odabranaPjesma)
	{
		InitializeComponent();
		BindingContext = new AudioPlayerViewModel(odabranaPjesma);

	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as AudioPlayerViewModel;
		if (viewModel != null)
		{
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
		}
	}



	private void Slider_DragStarted(object sender, EventArgs e)
	{
		var viewModel = BindingContext as AudioPlayerViewModel;
		if (viewModel != null)
		{
			viewModel.IsUserInteracting = true;
		}
	}

	private void Slider_DragCompleted(object sender, EventArgs e)
	{
		var viewModel = BindingContext as AudioPlayerViewModel;
		if (viewModel != null)
		{
			viewModel.IsUserInteracting = false;
		}
	}


	private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		var viewModel = BindingContext as AudioPlayerViewModel;

		if (viewModel == null) return;

		if (viewModel.IsUserInteracting)
		{
			var newPosition = e.NewValue;
			CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(newPosition)).ContinueWith((task) =>
			{
				viewModel.CurrentPosition = newPosition;
			});
		}




	}


}
