using MediaManager;
using MusicStreamingService;
using MusicStreamingService.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class AudioPlayerViewModel : INotifyPropertyChanged
{
	public Pjesma CurrentSong { get; private set; }
	public ICommand PlayPauseCommand { get; }
	public ICommand NextCommand { get; }
	public ICommand PreviousCommand { get; }

	private bool isPlaying;

	public string PlayPauseButtonText => isPlaying ? "⏸️" : "▶️";


	public AudioPlayerViewModel(Pjesma pjesma)
	{
		CurrentSong = pjesma;
		PlayPauseCommand = new Command(OnPlayPause);
		//NextCommand = new Command(OnNext);
		//PreviousCommand = new Command(OnPrevious);
		OnPropertyChanged(nameof(CurrentSong));
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


	private void OnNext()
	{
		
	}

	private void OnPrevious()
	{
		
	}

	public event PropertyChangedEventHandler PropertyChanged;
	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
