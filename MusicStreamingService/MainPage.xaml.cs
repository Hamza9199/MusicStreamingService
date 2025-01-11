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
	private bool _notifikacijaZakazanaAlbum = false;
	private bool _notifikacijaZakazanaPlaylist = false;
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

			if (viewModel.pjesma1.naziv == null)
			{
				Console.WriteLine("Pjesma nije postavljena.");
				return;
			}
				var request = new NotificationRequest
				{
					NotificationId = 1777,
					Title = "Pjesma za Dušu",
					Subtitle = "Pjesma dana",
					Image = new NotificationImage { FilePath = "AppIcon/ikona.jpg" },
					Description = $"{viewModel.pjesma1.naziv} - {viewModel.pjesma1.opis}",
					Schedule = new NotificationRequestSchedule
					{
						NotifyTime = DateTime.Now.AddSeconds(100),
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

	private void NotifikacijaAlbuma()
	{
		try
		{
			Console.WriteLine("Priprema notifikacije...");
			var viewModel = BindingContext as MainPageViewModel;

			if (viewModel.pjesma2.naziv == null)
			{
				Console.WriteLine("Pjesma nije postavljena.");
				return;
			}
			var request = new NotificationRequest
			{
				NotificationId = 1778,
				Title = "Album za Dušu",
				Subtitle = "Album dana",
				Image = new NotificationImage { FilePath = "AppIcon/ikona.jpg" },
				Description = $"{viewModel.pjesma2.naziv} ",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = DateTime.Now.AddSeconds(200),
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

	private void NotifikacijaPlayliste()
	{
		try
		{
			Console.WriteLine("Priprema notifikacije...");
			var viewModel = BindingContext as MainPageViewModel;

			if (viewModel.pjesma3.naziv == null)
			{
				Console.WriteLine("Pjesma nije postavljena.");
				return;
			}
			var request = new NotificationRequest
			{
				NotificationId = 1779,
				Title = "Playlista za Dušu",
				Subtitle = "Playlista dana",
				Image = new NotificationImage { FilePath = "AppIcon/ikona.jpg" },
				Description = $"{viewModel.pjesma3.naziv} ",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = DateTime.Now.AddSeconds(300),
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

		if (!_notifikacijaZakazanaAlbum)
		{
			NotifikacijaAlbuma();
		}

		if (!_notifikacijaZakazanaPlaylist)
		{
			NotifikacijaPlayliste();
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
