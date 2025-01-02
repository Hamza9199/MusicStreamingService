using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;
using System;

namespace MusicStreamingService.Views
{
	public partial class UrediPjesmu : ContentPage
	{
		public UrediPjesmu(Pjesma odabranaPjesma)
		{
			InitializeComponent();
			BindingContext = new UrediPjesmuViewModel(odabranaPjesma);
		}

		private void OnSelectImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediPjesmuViewModel viewModel)
			{
				viewModel.SelectImage();
			}
		}

		private void OnUploadImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediPjesmuViewModel viewModel)
			{
				viewModel.UploadImage();
			}
		}

		private void OnSelectAudioClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediPjesmuViewModel viewModel)
			{
				viewModel.SelectAudio();
			}
		}

		private void OnUploadAudioClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is UrediPjesmuViewModel viewModel)
			{
				viewModel.UploadAudio();
			}
		}
	}
}
