using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class UrediPlaylistu : ContentPage
{
	public UrediPlaylistu(PlayLista odabranaPlaylista)
	{
		InitializeComponent();
		BindingContext = new UrediPlaylistuViewModel(odabranaPlaylista);
	}

	private void OnSelectImageClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is UrediPlaylistuViewModel viewModel)
		{
			viewModel.SelectImage();
		}
	}

	private void OnUploadImageClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is UrediPlaylistuViewModel viewModel)
		{
			viewModel.UploadImage();
		}
	}

	private void OnSelectGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is UrediPlaylistuViewModel viewModel)
		{
			viewModel.SelectGif();
		}
	}

	private void OnUploadGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is UrediPlaylistuViewModel viewModel)
		{
			viewModel.UploadGif();
		}
	}
}
