using Microsoft.Maui.Controls;
using MusicStreamingService.Models;

namespace MusicStreamingService.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

	private async void OnLoginLabelTapped(object sender, EventArgs e)
	{
		if (Application.Current != null)
		{
			NavigationPage loginNavigationPage = new NavigationPage(new Login());
			Application.Current.MainPage = loginNavigationPage;
		}
	}

	private async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
		string? username = UsernameEntry.Text?.Trim();
		string? email = EmailEntry.Text?.Trim();
		string? password = PasswordEntry.Text?.Trim();
		string? confirmPassword = ConfirmPasswordEntry.Text?.Trim();

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
			string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
		{
			await DisplayAlert("Error", "All fields are required.", "OK");
			return;
		}

		if (password != confirmPassword)
		{
			await DisplayAlert("Error", "Passwords do not match.", "OK");
			return;
		}

		Korisnik newUser = new Korisnik(username, email, password);

		await DisplayAlert("Success", $"Welcome, {newUser.Username}!", "OK");

		if (Application.Current != null)
		{
			NavigationPage loginNavigationPage = new NavigationPage(new Login());
			Application.Current.MainPage = loginNavigationPage;
		}
	}
}
