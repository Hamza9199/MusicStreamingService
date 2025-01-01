namespace MusicStreamingService.Views;

using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.ViewModels;
using Newtonsoft.Json;
using System.Text.Json;

public partial class AudioPlayer : ContentPage
{
	public AudioPlayer(Pjesma odabranaPjesma)
	{
		InitializeComponent();
		BindingContext = new AudioPlayerViewModel(odabranaPjesma);

	}

	
}
