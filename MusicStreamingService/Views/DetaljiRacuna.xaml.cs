namespace MusicStreamingService.Views;

public partial class DetaljiRacuna : ContentPage
{
	public DetaljiRacuna()
	{
		InitializeComponent();
	}

	private void ObrisiRacun(object sender, TappedEventArgs e)
	{
		DisplayAlert("Obavijest", "Racun obrisan", "OK");
	}
}