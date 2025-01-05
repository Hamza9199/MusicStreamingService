using MediaManager;
using MusicStreamingService.Models;
using MusicStreamingService.Services;
using MusicStreamingService.ViewModels;
using MusicStreamingService.Views;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification;

namespace MusicStreamingService;

public partial class MainPage : ContentPage
{
	private readonly ILoginRepository _loginRepository;
	private bool _notifikacijaZakazana = false;
	public MainPage(ILoginRepository loginRepository)
	{
		InitializeComponent();
		_loginRepository = loginRepository;
		BindingContext = new MainPageViewModel();

	}
	private void Notifikacija()
	{
		try
		{
			Console.WriteLine("Priprema notifikacije...");
			var viewModel = BindingContext as MainPageViewModel;

			
				var request = new NotificationRequest
				{
					NotificationId = 1777,
					Title = "Pjesma za Dušu",
					Subtitle = "Pjesma dana",
					Image = new NotificationImage { FilePath = viewModel.pjesma1.putanjaSlika },
					Description = $"{viewModel.pjesma1.naziv} - {viewModel.pjesma1.opis}",
					Schedule = new NotificationRequestSchedule
					{
						NotifyTime = DateTime.Now.AddSeconds(10),
						NotifyRepeatInterval = TimeSpan.FromDays(1),
						RepeatType = NotificationRepeat.Daily
					},
					Android = new AndroidOptions
					{
						AutoCancel = true,
						LaunchAppWhenTapped = true
					},
				};

				LocalNotificationCenter.Current.Show(request);

				Console.WriteLine("Notifikacija poslana.");
			
			
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Greška pri slanju notifikacije: {ex.Message}");
		}
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (await _loginRepository.isUserAuthenticated())
		{
			await Shell.Current.GoToAsync("//MainTabs");
		}
		else
		{
			await Shell.Current.GoToAsync("//Aut");

		}
	}

	// ...

	public async void pitaj()
	{
		if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.Version.Major >= 13)
		{
			var status = await LocalNotificationCenter.Current.RequestNotificationPermission();
			
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		pitaj();

		if (!_notifikacijaZakazana)
		{
			Notifikacija();
		}
		

		var viewModel = BindingContext as MainPageViewModel;
		if (viewModel != null)
		{
			viewModel.CurrentSong = null;
			viewModel.CurrentKorisnik = null;
			viewModel.CurrentPlaylista = null;
			viewModel.CurrentAlbum = null;
		}

		Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
	}
	private void OnLogoutClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
		
	}

	private void OnLajkovanePjesmeClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new LajkovanePjesme());
	}

	private void ImageButton_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new LajkovanePjesme());
	}
}
