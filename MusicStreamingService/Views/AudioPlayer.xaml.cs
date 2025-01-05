namespace MusicStreamingService.Views;

using CommunityToolkit.Maui.Storage;
using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public partial class AudioPlayer : ContentPage
{
	IFileSaver fileSaver;
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

	private async void DownloadSong(object sender, EventArgs e)
	{
		var viewModel = BindingContext as AudioPlayerViewModel;

		if (viewModel?.CurrentSong == null)
		{
			await DisplayAlert("Greška", "Nije odabrana nijedna pjesma za preuzimanje.", "U redu");
			return;
		}

		string sourcePath = viewModel.CurrentSong.putanjaAudio;
		string fileName = System.IO.Path.GetFileName(sourcePath);

		try
		{
			using (var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
			{
				var result = await fileSaver.SaveAsync(fileName, stream);

				if (result.IsSuccessful)
				{
					await DisplayAlert("Uspjeh", "Pjesma je uspješno preuzeta.", "U redu");
					viewModel.CurrentSong.putanjaAudio = result.FilePath;
				}
				else
				{
					await DisplayAlert("Greška", "Nije moguće spremiti pjesmu.", "U redu");
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Greška", $"Došlo je do greške prilikom preuzimanja: {ex.Message}", "U redu");
		}
	}


}
