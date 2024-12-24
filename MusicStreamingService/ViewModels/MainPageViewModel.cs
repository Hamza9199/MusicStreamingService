using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediaManager;
using YoutubeExplode;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace MusicStreamingService.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<Pjesma> Songs { get; set; }
		public ICommand SearchCommand { get; }
		public ICommand PlayPauseCommand { get; }
		public ICommand NextCommand { get; }
		public ICommand PreviousCommand { get; }
		public string PlayPauseButtonText => IsPlaying ? "⏸️" : "▶️";

		private Pjesma _currentSong;
		public Pjesma CurrentSong
		{
			get => _currentSong;
			set
			{
				_currentSong = value;
				OnPropertyChanged();
			}
		}

		private bool _isPlaying;
		public bool IsPlaying
		{
			get => _isPlaying;
			set
			{
				_isPlaying = value;
				OnPropertyChanged();
			}
		}

		public MainPageViewModel()
		{
			CrossMediaManager.Current.Init();
			Songs = new ObservableCollection<Pjesma>();
			SearchCommand = new Command<string>(OnSearch);
			PlayPauseCommand = new Command(OnPlayPause);
			NextCommand = new Command(OnNext);
			PreviousCommand = new Command(OnPrevious);

			LoadSongs();
		}


		private void LoadSongs()
		{
			Songs.Add(new Pjesma { Title = "Song 1", Artist = "Artist 1", Url = "https://www.youtube.com/watch?v=tMSWpIKflrE", AlbumArt = "https://example.com/art1.jpg" });
			Songs.Add(new Pjesma { Title = "Song 2", Artist = "Artist 2", Url = "https://www.youtube.com/watch?v=tMSWpIKflrE", AlbumArt = "https://example.com/art2.jpg" });
			Songs.Add(new Pjesma { Title = "Song 3", Artist = "Artist 3", Url = "https://www.youtube.com/watch?v=tMSWpIKflrE", AlbumArt = "https://example.com/art3.jpg" });
		}

		private void OnSearch(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return;
			}

			var filteredSongs = Songs.Where(song =>
				song.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				song.Artist.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

			Songs.Clear();
			foreach (var song in filteredSongs)
			{
				Songs.Add(song);
			}
		}

		private async Task<string> GetAudioFromYouTube(string videoUrl)
		{
			try
			{
				var youtube = new YoutubeClient();
				var video = await youtube.Videos.GetAsync(videoUrl);
				var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
				var audioStreamInfo = streamManifest.GetAudioOnlyStreams()
													 .OrderByDescending(s => s.Bitrate)
													 .FirstOrDefault();

				if (audioStreamInfo != null)
				{
					var tempPath = Path.Combine(Path.GetTempPath(), $"{video.Id}.mp3");
					await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, tempPath);
					System.Diagnostics.Debug.WriteLine($"Audio downloaded to: {tempPath}"); 
					return tempPath;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error downloading audio: {ex.Message}");
			}
			return null;
		}



		private async void OnPlayPause()
		{
			if (CurrentSong == null)
			{
				await Application.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			if (IsPlaying)
			{
				await CrossMediaManager.Current.Pause();
				IsPlaying = false;
			}
			else
			{
				

				string audioPath2 = await GetAudioFromYouTube(CurrentSong.Url);
				string audioPath = "https://firebasestorage.googleapis.com/v0/b/trailerflix-25df2.appspot.com/o/items%2Ftest.mp3?alt=media&token=c7aa63d1-9c64-4660-af02-9c8809ed7c90";

				

				try
				{
					System.Diagnostics.Debug.WriteLine($"Attempting to play: {audioPath}");
					await CrossMediaManager.Current.Play(audioPath);
					IsPlaying = true;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Error playing audio: {ex.Message}");
					await Application.Current.MainPage.DisplayAlert($"Greška", "Ne mogu reproducirati pjesmu.", "U redu");
				}
			}
			OnPropertyChanged(nameof(PlayPauseButtonText));
		}

		private void OnNext()
		{
			if (CurrentSong == null || Songs.Count == 0)
				return;

			var currentIndex = Songs.IndexOf(CurrentSong);
			if (currentIndex < Songs.Count - 1)
			{
				CurrentSong = Songs[currentIndex + 1];
				CrossMediaManager.Current.Play(CurrentSong.Url);
			}
		}

		private void OnPrevious()
		{
			if (CurrentSong == null || Songs.Count == 0)
				return;

			var currentIndex = Songs.IndexOf(CurrentSong);
			if (currentIndex > 0)
			{
				CurrentSong = Songs[currentIndex - 1];
				CrossMediaManager.Current.Play(CurrentSong.Url);
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
