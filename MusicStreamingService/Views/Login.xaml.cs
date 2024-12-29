using Microsoft.Maui.Controls;

namespace MusicStreamingService.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	private async void OnSignUpLabelTapped(object sender, EventArgs e)
	{
		if (Application.Current != null)
		{
			await Shell.Current.GoToAsync("//Register");
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

		if (username == "hamza" && password == "hamza")
		{
			await DisplayAlert("Success", "Login successful!", "Ok");
			if (Application.Current != null)
			{
				await Shell.Current.GoToAsync("//MainPage");
			}
		}
		else
		{
			await DisplayAlert("Error", "Invalid username or password.", "OK");
		}
	}
}
