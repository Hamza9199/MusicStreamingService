using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class KreirajPlaylistu : ContentPage
{
	public KreirajPlaylistu()
	{
		InitializeComponent();
		BindingContext = new KreirajPlaylistuViewModel();

	}
}