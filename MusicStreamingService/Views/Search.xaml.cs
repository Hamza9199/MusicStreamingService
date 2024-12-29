using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Search : ContentPage
{
	public Search()
	{
		InitializeComponent();
		BindingContext = new SearchViewModel();

	}
}