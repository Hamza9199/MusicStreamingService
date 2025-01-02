using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class KreirajAlbum : ContentPage
{
	public KreirajAlbum()
	{
		InitializeComponent();
		BindingContext = new KreirajAlbumViewModel();

	}

	private  void OnSelectImageClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajAlbumViewModel viewModel)
		{
			viewModel.SelectImage();
		}
	}

	private void OnUploadImageClicked(object sender, EventArgs e)
	{
		if(this.BindingContext is KreirajAlbumViewModel viewModel)
			{
			viewModel.UploadImage();
		}

	}

	private void OnSelectGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajAlbumViewModel viewModel)
		{
			viewModel.SelectGif();
		}
	}

	private void OnUploadGifClicked(object sender, EventArgs e)
	{
		if (this.BindingContext is KreirajAlbumViewModel viewModel)
		{
			viewModel.UploadGif();
		}


	}
}