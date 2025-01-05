using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class DetaljiRacuna : ContentPage
{
	public DetaljiRacuna()
	{
		InitializeComponent();
		BindingContext = new DetaljiRacunaViewModel();

	}

	private void ObrisiRacun(object sender, TappedEventArgs e)
	{
		DisplayAlert("Obavijest", "Racun obrisan", "OK");
	}
}