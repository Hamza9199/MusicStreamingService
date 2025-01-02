using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class KreirajPretplatu : ContentPage
{
	public KreirajPretplatu()
	{
		InitializeComponent();
		BindingContext = new KreirajPretplatuViewModel();

	}
}