using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class OfflineMuzika : ContentPage
{
	public OfflineMuzika()
	{
		InitializeComponent();

		BindingContext = new OfflineMuzikaViewModel();
	}
}