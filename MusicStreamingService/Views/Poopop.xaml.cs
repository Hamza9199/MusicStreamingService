using CommunityToolkit.Maui.Views;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Poopop : Popup
{
	public Poopop(Pjesma odabranaPjesma)
	{
		InitializeComponent();
		BindingContext = new PoopopviewModel(odabranaPjesma);
	}
}