using Microsoft.Maui;
using Microsoft.Maui.Controls;
using MusicStreamingService.Models;
using MusicStreamingService.Services;

namespace MusicStreamingService.Views;

public partial class Register : ContentPage
{
	readonly ILoginRepository _loginRepository;

	public Register(ILoginRepository loginRepository)
	{
		_loginRepository = loginRepository;
		InitializeComponent();
	}

	private async void OnLoginLabelTapped(object sender, EventArgs e)
	{
		if (Application.Current != null)
		{
			await Shell.Current.GoToAsync("//Aut/Login");
		}
	}

	private async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
		string? email = EmailEntry.Text?.Trim();
		string? password = PasswordEntry.Text?.Trim();
		string? confirmPassword = ConfirmPasswordEntry.Text?.Trim();
		string? ime = ImeEntry.Text?.Trim();
		string? prezime = PrezimeEntry.Text?.Trim();


		if (string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(email) ||
			string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(prezime)) 
		{
			await DisplayAlert("Error", "All fields are required.", "OK");
			return;
		}

		if (password != confirmPassword)
		{
			await DisplayAlert("Error", "Passwords do not match.", "OK");
			return;
		}

		Korisnik newUser = new Korisnik { prezime = prezime, ime = ime, email = email, lozinka = password };


		var error = await _loginRepository.Registracija(newUser);
		if (error != null)
		{
			await DisplayAlert("Error", error, "OK");
			return;
		}
		else
		{
			await DisplayAlert("Success", "Registration successful!", "Ok");

			if (Application.Current != null)
			{
				await Shell.Current.GoToAsync("//Aut/Login");
			}
		}

		
	}
}
