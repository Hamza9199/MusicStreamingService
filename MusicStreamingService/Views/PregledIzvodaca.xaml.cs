using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class PregledIzvodaca : ContentPage
{
	public PregledIzvodaca()
	{
		InitializeComponent();

		BindingContext = new PregledIzvodacaViewModel();

	}
}