using CommunityToolkit.Maui.Views;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Poop : Popup
{
	public Poop(Models.Album odabraniAlbum)
	{
		InitializeComponent();
		BindingContext = new PoopviewModel(odabraniAlbum);

	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		this.Close();
	}
}