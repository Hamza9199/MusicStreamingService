using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class UrediPlaylistu : ContentPage
{
	public UrediPlaylistu()
	{
		InitializeComponent();
		BindingContext = new UrediPlaylistuViewModel();

	}
}