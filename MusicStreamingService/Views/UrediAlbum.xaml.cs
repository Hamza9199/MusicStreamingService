using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views
{
	public partial class UrediAlbum : ContentPage
	{
		public UrediAlbum(Models.Album odabraniAlbum)
		{
			InitializeComponent();
			BindingContext = new UrediAlbumViewModel(odabraniAlbum);
		}

		private void OnSelectImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediAlbumViewModel viewModel)
			{
				viewModel.SelectImage();
			}
		}

		private void OnUploadImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediAlbumViewModel viewModel)
			{
				viewModel.UploadImage();
			}
		}

		private void OnSelectGifClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediAlbumViewModel viewModel)
			{
				viewModel.SelectGif();
			}
		}

		private void OnUploadGifClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediAlbumViewModel viewModel)
			{
				viewModel.UploadGif();
			}
		}
	}
}
