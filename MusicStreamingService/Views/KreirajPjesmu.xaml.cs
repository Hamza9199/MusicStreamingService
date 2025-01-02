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
		
		private void OnSelectImageClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.ImageUpload();
			}
			else
			{
				Debug.WriteLine("BindingContext is not of type KreirajPjesmuViewModel");
			}
		}

		private void OnSelectAudioClicked(object sender, EventArgs e)
		{
			if (this.BindingContext is KreirajPjesmuViewModel viewModel)
			{
				viewModel.AudioUpload();
			}
			else
			{
				Debug.WriteLine("BindingContext is not of type KreirajPjesmuViewModel");
			}
		}
	}
}