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

		Debug.WriteLine("Email: " + korisnickoIme);
		Debug.WriteLine("Password: " + password);
		if (string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(password))
		{
			await DisplayAlert("Error", "Please enter both username and password.", "OK");
			return;
		}

		Korisnik korisnik = new Korisnik
		{
			email = korisnickoIme,
			password = password
		};
		Debug.WriteLine("Korisnik: " + korisnik);
		var error = await _loginRepository.Login(korisnik);
		Debug.WriteLine(error);


		if (string.IsNullOrEmpty(error)) 
		{
			await DisplayAlert("Success", "Login successful!", "OK");
			MessagingCenter.Send<Login>(this, "admin");
			if (Application.Current != null)
			{
				await Shell.Current.GoToAsync("//MainTabs");
			}
		}
		else
		{
			await DisplayAlert("Error", error, "OK");
		}


	}
}
