using Microsoft.Maui.Controls;
using MusicStreamingService.Models;
using MusicStreamingService.Services;
using System.Diagnostics;

namespace MusicStreamingService.Views;

public partial class Login : ContentPage
{

	readonly ILoginRepository _loginRepository;
	public Login(ILoginRepository loginRepository)
	{
		InitializeComponent();
		_loginRepository = loginRepository;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
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
		string? korisnickoIme = KorisnickoImeEntry.Text?.Trim();
		string? password = PasswordEntry.Text?.Trim();

		if (string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(password))
		{
			await DisplayAlert("Error", "Please enter both username and password.", "OK");
			return;
		}

		Korisnik korisnik = new Korisnik
		{
			KorisnickoIme = korisnickoIme,
			lozinka = password
		};

		var error = _loginRepository.Login(korisnik);
		Debug.WriteLine(error);

		/*
		if (error != null)
		{
			await DisplayAlert("Error", await error, "OK");
			return;
		}
		else
		{
			await DisplayAlert("Success", "Login successful!", "Ok");
			MessagingCenter.Send<Login>(this, "admin");
			if (Application.Current != null)
			{
				await Shell.Current.GoToAsync("//MainTabs");
			}
		}*/


		if (korisnickoIme == "h" && password == "h")
		{
			await DisplayAlert("Success", "Login successful!", "Ok");
			MessagingCenter.Send<Login>(this, "admin");
			await SecureStorage.SetAsync("token", korisnik.ToString());
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
