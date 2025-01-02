using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class KreirajPlaylistu : ContentPage
{
	public KreirajPlaylistu()
	{
		InitializeComponent();
		BindingContext = new KreirajPlaylistuViewModel();

	}

	private void OnSelectImageClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajPlaylistuViewModel viewModel)
		{
			viewModel.SelectImage();
		}
	}

	private  void OnUploadImageClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajPlaylistuViewModel viewModel)
		{
			viewModel.UploadImage();
		}

	}

	private void OnSelectGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajPlaylistuViewModel viewModel)
		{
			viewModel.SelectGif();
		}
	}

	private void OnUploadGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajPlaylistuViewModel viewModel)
		{
			viewModel.UploadGif();
		}


	}

}