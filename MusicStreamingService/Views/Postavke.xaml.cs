using MusicStreamingService.Services;
using MusicStreamingService.ViewModels;

namespace MusicStreamingService.Views;

public partial class Postavke : ContentPage
{
	readonly ILoginRepository _loginRepository;

	public Postavke(ILoginRepository loginRepository)
	{
		InitializeComponent();

		_loginRepository = loginRepository;
		BindingContext = new PostavkeViewModel();

	}

	private async void Odjava_Clicked(object sender, EventArgs e)
	{
		_loginRepository.LogoutAsync();
		await Shell.Current.GoToAsync("//Aut");
	}


}