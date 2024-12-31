using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Postavke : ContentPage
{
	public Postavke()
	{
		InitializeComponent();
		BindingContext = new PostavkeViewModel();

	}

	private async void Odjava_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//Aut");
	}


}