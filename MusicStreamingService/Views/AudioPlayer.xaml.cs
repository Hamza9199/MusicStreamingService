namespace MusicStreamingService.Views;

using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;


public partial class AudioPlayer : ContentPage
{
	public AudioPlayer(Pjesma odabranaPjesma)
	{
		InitializeComponent();
		BindingContext = new AudioPlayerViewModel(odabranaPjesma);
	}


}