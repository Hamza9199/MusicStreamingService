using MediaManager;
using MusicStreamingService;
using MusicStreamingService.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading;
using System;
using System.Timers;
using System.Text;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Text.Json;


public class AudioPlayerViewModel : INotifyPropertyChanged
{
	public readonly HttpClient _httpClient;

	public DobiveniKorisnik Korisnik { get; set; }

	public Pjesma CurrentSong { get; set; }
	public ICommand PlayPauseCommand { get; }
	public ICommand RewindCommand { get; }
	public ICommand ForwardCommand { get; }
	public ICommand VolumeUpCommand { get; }
	public ICommand VolumeDownCommand { get; }

	public ICommand Like { get; }

	public ICommand Ponavljaj { get; }

	public ICommand Download { get; }

	public ICommand Dodaj { get; }

	private bool ponavljaj = false;

	private bool isPlaying;
	public bool IsUserInteracting { get; set; }

	private System.Threading.Timer timer;
	private double currentPosition;
	private double volume = 0.5;
	public string PlayPauseButtonText => isPlaying ? "⏸️" : "▶️";


	public double CurrentPosition
	{
		get => currentPosition;
		set
		{
			if (value >= 0 && value <= SongDuration && Math.Abs(currentPosition - value) > 0.1)
			{
				currentPosition = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(CurrentTime));
			}
		}
	}



	public string CurrentTime => TimeSpan.FromSeconds(currentPosition).ToString(@"mm\:ss");

	private double songDuration;

	public double SongDuration
	{
		get => songDuration;
		private set
		{
			songDuration = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(SongDurationString));
		}
	}

	public string SongDurationString => TimeSpan.FromSeconds(SongDuration).ToString(@"mm\:ss");

	public double Volume
	{
		get => volume;
		set
		{
			volume = value;
			CrossMediaManager.Current.Volume.CurrentVolume = (int)(volume * CrossMediaManager.Current.Volume.MaxVolume);
			OnPropertyChanged();
		}
	}

	public AudioPlayerViewModel(Pjesma pjesma)
	{
		_httpClient = new HttpClient
		{
			BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/")
		};

		_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
			"Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

		CurrentSong = pjesma;
		PlayPauseCommand = new Command(OnPlayPause);
		RewindCommand = new Command(() => Seek(-5));
		ForwardCommand = new Command(() => Seek(5));
		VolumeUpCommand = new Command(() => Volume += 0.1);
		VolumeDownCommand = new Command(() => Volume -= 0.1);
		Like = new Command(OnLike);
		Korisnik = new DobiveniKorisnik();
		ponavljaj = false;
		IsUserInteracting = false;
		Ponavljaj = new Command(() =>
		{
			ponavljaj = !ponavljaj;
			OnPropertyChanged(nameof(ponavljaj));
		});
		Download = new Command(OnDownload);
		timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
		Dodaj = new Command(OnDodajUPlaylistu);

		OnPropertyChanged(nameof(CurrentSong));
		LoadTokenData();

	}

	private async void OnDodajUPlaylistu() { 
		await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma dodana u playlistu", "U redu");
	}

	private async void OnDownload()
	{
		try
		{
			string localPath = Path.Combine(FileSystem.AppDataDirectory, CurrentSong.naziv + ".mp3");
			if (File.Exists(localPath))
			{
				Debug.WriteLine("Pjesma je već skinuta.");
				return;
			}

			var response = await _httpClient.GetAsync(CurrentSong.putanjaAudio);
			response.EnsureSuccessStatusCode();

			using (var fileStream = File.Create(localPath))
			{
				await response.Content.CopyToAsync(fileStream);
			}

			Debug.WriteLine($"Pjesma je uspješno skinuta: {localPath}");
			await App.Current.MainPage.DisplayAlert("Obavijest", "Pjesma uspjesno skinuta.", "U redu");


		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Greška prilikom preuzimanja pjesme: {ex.Message}");
			await App.Current.MainPage.DisplayAlert("Greška", "Pjesma nije skinuta.", "U redu");

		}
	}


	private async void LoadTokenData()
	{
		try
		{
			var tokenJson = await SecureStorage.GetAsync("token");
			if (!string.IsNullOrEmpty(tokenJson))
			{
				var token = System.Text.Json.JsonSerializer.Deserialize<DobiveniKorisnik>(tokenJson, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (token != null)
				{
					foreach (var claim in token.GetType().GetProperties())
					{
						Debug.WriteLine($"Claim: {claim.Name} = {claim.GetValue(token)}");
					}

					Korisnik.Id = token.Id;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Greška pri učitavanju tokena: {ex.Message}");
		}
	}

	private async void OnLike()
	{
		LoadTokenData();

		if (CurrentSong == null)
		{
			await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
			return;
		}

		KorisnikPjesma korisnikPjesma = new KorisnikPjesma
		{
			korisnikID = Korisnik.Id,
			pjesmaID = CurrentSong.id,
		};
		try
		{
			var response = await _httpClient.PostAsJsonAsync("api/KorisnikPjesmaControllerAPI", korisnikPjesma);
			Debug.WriteLine(response);
			var responseContent = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(responseContent);
			if (response.IsSuccessStatusCode)
			{
				CurrentSong.brojLajkova++;
				OnPropertyChanged(nameof(CurrentSong));
				await App.Current.MainPage.DisplayAlert("Upjeh", "Pjesma lajkovana.", "U redu");

			}
			else
			{
				Debug.WriteLine(response);
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu lajkovati pjesmu.", "U redu");
			}
		}
		catch (Exception ex)
		{
			

			Debug.WriteLine($"Greška prilikom lajkovanja: {ex.Message}");
			await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu lajkovati pjesmu.", "U redu");

		}

	}



	private void TimerCallback(object state)
	{
		if (CrossMediaManager.Current.IsPlaying())
		{
			var position = CrossMediaManager.Current.Position;
			var duration = CrossMediaManager.Current.Duration;

			if ((position >= duration && duration > TimeSpan.Zero) ||
			(duration - position <= TimeSpan.FromSeconds(1) && duration > TimeSpan.Zero))
			{
				isPlaying = false;
				OnPropertyChanged(nameof(PlayPauseButtonText));

				if (ponavljaj)
				{
					string audioPath = GetAudioPath(CurrentSong);
					CrossMediaManager.Current.Play(audioPath);
					isPlaying = true;
					OnPropertyChanged(nameof(PlayPauseButtonText));
				}
				return;
			}

			if (position != TimeSpan.Zero && duration != TimeSpan.Zero)
			{
				CurrentPosition = position.TotalSeconds;
				SongDuration = duration.TotalSeconds;
			}

			

			OnPropertyChanged(nameof(CurrentTime));
			OnPropertyChanged(nameof(SongDurationString));
		}
	}




	private string GetAudioPath(Pjesma pjesma)
	{
		if (pjesma == null)
		{
			System.Diagnostics.Debug.WriteLine("Greška: Nije selektovana nijedna pjesma.");
			return string.Empty;
		}

		return pjesma.putanjaAudio;
	}

	private async void OnPlayPause()
	{
		if (CurrentSong == null)
		{
			await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
			return;
		}

		var mediaManager = CrossMediaManager.Current;


		if (isPlaying)
		{
			await mediaManager.Pause();
			isPlaying = false;
		}
		else
		{
			string audioPath = GetAudioPath(CurrentSong);
			try
			{
				System.Diagnostics.Debug.WriteLine($"Pokušaj reprodukcije: {audioPath}");

				if (mediaManager.IsStopped())
				{
					await mediaManager.Play(audioPath);
				}
				else
				{
					await mediaManager.Play();
				}

				isPlaying = true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom reprodukcije: {ex.Message}");
				await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
			}
		}

		OnPropertyChanged(nameof(PlayPauseButtonText));
	}

	private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
	{
		var newPosition = e.NewValue;

		CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(newPosition));
	}

	private void Seek(int seconds)
	{
		CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(CurrentPosition + seconds));
	}

	public event PropertyChangedEventHandler PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
