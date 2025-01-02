using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class UrediPjesmu : ContentPage
{
	public UrediPjesmu()
	{
		InitializeComponent();
		BindingContext = new UrediPjesmuViewModel();

	}
}