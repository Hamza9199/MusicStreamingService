using MediaManager;
using MusicStreamingService;
using MusicStreamingService.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading;
using System;
using System.Timers;


public class AudioPlayerViewModel : INotifyPropertyChanged
{

	public Pjesma CurrentSong { get; private set; }
	public ICommand PlayPauseCommand { get; }
	public ICommand RewindCommand { get; }
	public ICommand ForwardCommand { get; }
	public ICommand VolumeUpCommand { get; }
	public ICommand VolumeDownCommand { get; }

	public Command<double> SliderValueChangedCommand { get; }

	private bool isPlaying;
	private System.Threading.Timer timer;
	private double currentPosition;
	private double volume = 0.5;
	public string PlayPauseButtonText => isPlaying ? "⏸️" : "▶️";


	public double CurrentPosition
	{
		get => currentPosition;
		set
		{
			currentPosition = value;
			OnPropertyChanged();
		}
	}

	public string CurrentTime => TimeSpan.FromSeconds(currentPosition).ToString(@"mm\:ss");

	public double SongDuration { get; private set; }
	public string SongDurationString => TimeSpan.FromSeconds(SongDuration).ToString(@"mm\:ss");

	public double Volume
	{
		get => volume;
		set
		{
			volume = value;
			CrossMediaManager.Current.Volume.CurrentVolume = (int)volume;
			OnPropertyChanged();
		}
	}

	public AudioPlayerViewModel(Pjesma pjesma)
	{
		CurrentSong = pjesma;
		PlayPauseCommand = new Command(OnPlayPause);
		RewindCommand = new Command(() => Seek(-5));
		ForwardCommand = new Command(() => Seek(5));
		VolumeUpCommand = new Command(() => Volume += 0.1);
		VolumeDownCommand = new Command(() => Volume -= 0.1);
		SliderValueChangedCommand = new Command<double>((newValue) =>
		{
			CrossMediaManager.Current.SeekTo(TimeSpan.FromSeconds(newValue));
		});

		timer = new System.Threading.Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

		OnPropertyChanged(nameof(CurrentSong));
	}

	private void TimerCallback(object state)
	{
		CurrentPosition = CrossMediaManager.Current.Position.TotalSeconds;
		SongDuration = CrossMediaManager.Current.Duration.TotalSeconds;
		OnPropertyChanged(nameof(CurrentTime));
		OnPropertyChanged(nameof(SongDurationString));
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
