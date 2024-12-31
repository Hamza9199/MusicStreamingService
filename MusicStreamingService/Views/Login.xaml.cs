using Microsoft.Maui.Controls;
using MusicStreamingService.Models;
using MusicStreamingService.Services;

namespace MusicStreamingService.Views;

public partial class Login : ContentPage
{

	readonly ILoginRepository _loginRepository = new LoginService();
	public Login()
	{
		InitializeComponent();
	}

	private async void OnSignUpLabelTapped(object sender, EventArgs e)
	{
		if (Application.Current != null)
		{
			await Shell.Current.GoToAsync("//Aut/Register");
		}
	}

	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
		string? username = UsernameEntry.Text?.Trim();
		string? password = PasswordEntry.Text?.Trim();

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			await DisplayAlert("Error", "Please enter both username and password.", "OK");
			return;
		}


		Korisnik korisnikInfo = await _loginRepository.Login(username, password);


		if (username == "h" && password == "h")
		{
			await DisplayAlert("Success", "Login successful!", "Ok");
			MessagingCenter.Send<Login>(this, "admin");
			if (Application.Current != null)
			{
				//Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
				await Shell.Current.GoToAsync("//MainTabs");

			}
		}
		else
		{
			await DisplayAlert("Error", "Invalid username or password.", "OK");
		}
	}
}
