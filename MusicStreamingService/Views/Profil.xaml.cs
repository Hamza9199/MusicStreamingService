using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Profil : ContentPage
{
	public Profil()
	{
		InitializeComponent();
		BindingContext = new ProfilViewModel();
	}
}