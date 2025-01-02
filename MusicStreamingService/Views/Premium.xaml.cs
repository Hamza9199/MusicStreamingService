using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Premium : ContentPage
{
	public Premium()
	{
		InitializeComponent();
		BindingContext = new PremiumViewModel();
	}

	private void IdiNaPretplatu(object sender, EventArgs e)
	{
		Navigation.PushAsync(new KreirajPretplatu());

	}
}