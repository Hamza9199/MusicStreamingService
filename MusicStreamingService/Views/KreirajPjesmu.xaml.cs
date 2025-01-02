using MusicStreamingService.ViewModels;
using System;
using System.Diagnostics;

namespace MusicStreamingService.Views
{
	public partial class KreirajPjesmu : ContentPage
	{
		public KreirajPjesmu()
		{
			InitializeComponent();
			BindingContext = new KreirajPjesmuViewModel();
		}

		private async void OnSelectImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.SelectImage();
			}
		}

		private async void OnUploadImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.UploadImage();
			}
		}

		private void OnSelectAudioClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.SelectAudio();
			}
		}

		private void OnUploadAudioClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.UploadAudio();
			}
		}

	}
}