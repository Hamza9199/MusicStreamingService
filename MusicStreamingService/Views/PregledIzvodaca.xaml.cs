using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class PregledIzvodaca : ContentPage
{
	public PregledIzvodaca(DobiveniKorisnik odabraniKorisnik)
	{
		InitializeComponent();

		BindingContext = new PregledIzvodacaViewModel(odabraniKorisnik);

	}
}