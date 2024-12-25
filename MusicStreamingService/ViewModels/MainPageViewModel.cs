using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MediaManager;

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
			Songs.Add(new Pjesma(
				ID: 1,
				albumID: 1,
				redniBrojUAlbumu: 1,
				naziv: "BASS",
				opis: "Jala Brat",
				datumObjave: DateOnly.FromDateTime(DateTime.Now),
				trajanjeSekunde: 180,
				javno: true,
				odobreno: true,
				putanjaAudio: "https://firebasestorage.googleapis.com/v0/b/trailerflix-25df2.appspot.com/o/items%2Ftest.mp3?alt=media&token=c7aa63d1-9c64-4660-af02-9c8809ed7c90",
				putanjaSlika: "https://i.scdn.co/image/ab67616d0000b273a8aa97fb6e61f7e092c166f6",
				putanjaGif: "https://i.scdn.co/image/ab67616d0000b273a8aa97fb6e61f7e092c166f6",
				brojReprodukcija: 0,
				brojLajkova: 0,
				jezikPjesme: "Bosanski",
				licenca: "Standardna licenca",
				eksplicitniSadrzaj: false,
				tekst: "null",
				kreiranDatumVrijeme: DateTime.Now
			));
			Songs.Add(new Pjesma(
				ID: 2,
				albumID: 1,
				redniBrojUAlbumu: 2,
				naziv: "F1 theme",
				opis: "Brian Tyler",
				datumObjave: DateOnly.FromDateTime(DateTime.Now),
				trajanjeSekunde: 180,
				javno: true,
				odobreno: true,
				putanjaAudio: "https://firebasestorage.googleapis.com/v0/b/trailerflix-25df2.appspot.com/o/items%2FF1%20theme%20by%20Brian%20Tyler.mp3?alt=media&token=5de7f505-809e-48b4-8f3f-d249397afe1b",
				putanjaSlika: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmBY8mn2_qmAmgCS6C3Ify_WWmVWmXOdpfsw&s",
				putanjaGif: "",
				brojReprodukcija: 0,
				brojLajkova: 0,
				jezikPjesme: "Bosanski",
				licenca: "Standardna licenca",
				eksplicitniSadrzaj: false,
				tekst: "null",
				kreiranDatumVrijeme: DateTime.Now
				));

		}


		private void OnSearch(string query)
		{
			if (string.IsNullOrWhiteSpace(query)) return;

			var filteredSongs = Songs.Where(song =>
				song.naziv.Contains(query, StringComparison.OrdinalIgnoreCase) ||
				(song.opis != null && song.opis.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();

			Songs.Clear();
			foreach (var song in filteredSongs)
			{
				Songs.Add(song);
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

			if (IsPlaying)
			{
				await CrossMediaManager.Current.Pause();
				IsPlaying = false;
			}
			else
			{
				string audioPath = GetAudioPath(CurrentSong);
				try
				{
					System.Diagnostics.Debug.WriteLine($"Pokušaj reprodukcije: {audioPath}");
					await CrossMediaManager.Current.Play(audioPath);
					IsPlaying = true;
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
			if (CurrentSong == null || Songs.Count == 0) return;

			var currentIndex = Songs.IndexOf(CurrentSong);
			if (currentIndex < Songs.Count - 1)
			{
				CurrentSong = Songs[currentIndex + 1];
				CrossMediaManager.Current.Play(CurrentSong.putanjaAudio);
			}
		}

		private void OnPrevious()
		{
			if (CurrentSong == null || Songs.Count == 0) return;

			var currentIndex = Songs.IndexOf(CurrentSong);
			if (currentIndex > 0)
			{
				CurrentSong = Songs[currentIndex - 1];
				CrossMediaManager.Current.Play(CurrentSong.putanjaAudio);
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
