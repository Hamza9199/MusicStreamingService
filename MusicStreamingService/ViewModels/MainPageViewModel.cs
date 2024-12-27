using MusicStreamingService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MediaManager;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MusicStreamingService.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{

		private readonly HttpClient _httpClient;

		public ObservableCollection<Pjesma> Songs { get; set; } 

		public ObservableCollection<Album> Albumi { get; set; }

		public ObservableCollection<HistorijaSlusanja> historijeSlusanja { get; set; }

		public ObservableCollection<Korisnik> korisnici { get; set; }

		public ObservableCollection<Komentar> komentari { get; set; }

		public ObservableCollection<PlayLista> playListe { get; set; }

		public ObservableCollection<Pretplata> pretplate { get; set; }

		public ObservableCollection<Zanr> zanrovi { get; set; }

		public ObservableCollection<StatistikaReprodukcije> statistikeReprodukcije { get; set; }

		public ObservableCollection<PjesmaZanr> pjesmeZanrovi { get; set; }

		public ObservableCollection<PjesmaPlayLista> pjesmePlayListe { get; set; }

		public ObservableCollection<PratilacKorisnik> pracenja { get; set; }

		public ObservableCollection<ObnovaPretplate> obnovePretplate { get; set; }

		public ObservableCollection<KorisnikPretplata> korisniciPretplate { get; set; }

		public ObservableCollection<KorisnikPlayLista> korisniciPlayListe { get; set; }

		public ObservableCollection<KorisnikPjesma> korisniciPjesme { get; set; }

		public ObservableCollection<KorisnikAlbum> korisniciAlbumi { get; set; }

		public ObservableCollection<IzvodjacPjesma> izvodjaciPjesme { get; set; }


		public ICommand SearchCommand { get; } 
		public ICommand PlayPauseCommand { get; }
		public ICommand NextCommand { get; } 
		public ICommand PreviousCommand { get; }

		public ICommand SelectSongCommand { get; }

		private string _pjesmiceText;

		public string PlayPauseButtonText => IsPlaying ? "⏸️" : "▶️"; 

		private Pjesma _currentSong;

		public string PjesmiceText
		{
			get => _pjesmiceText;
			set
			{
				_pjesmiceText = value;
				OnPropertyChanged();
			}
		}

		public Pjesma SelectedSong
		{
			get => _selectedSong;
			set
			{
				if (_selectedSong != value)
				{
					_selectedSong = value;
					OnPropertyChanged();
					CurrentSong = _selectedSong;
				}
			}
		}

		private Pjesma _selectedSong;


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

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/") 
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Songs = new ObservableCollection<Pjesma>();
			Albumi = new ObservableCollection<Album>();
			historijeSlusanja = new ObservableCollection<HistorijaSlusanja>();
			korisnici = new ObservableCollection<Korisnik>();
			komentari = new ObservableCollection<Komentar>();
			playListe = new ObservableCollection<PlayLista>();
			pretplate = new ObservableCollection<Pretplata>();
			zanrovi = new ObservableCollection<Zanr>();
			statistikeReprodukcije = new ObservableCollection<StatistikaReprodukcije>();
			pjesmeZanrovi = new ObservableCollection<PjesmaZanr>();
			pjesmePlayListe = new ObservableCollection<PjesmaPlayLista>();
			pracenja = new ObservableCollection<PratilacKorisnik>();
			obnovePretplate = new ObservableCollection<ObnovaPretplate>();
			korisniciPretplate = new ObservableCollection<KorisnikPretplata>();
			korisniciPlayListe = new ObservableCollection<KorisnikPlayLista>();
			korisniciPjesme = new ObservableCollection<KorisnikPjesma>();
			korisniciAlbumi = new ObservableCollection<KorisnikAlbum>();
			izvodjaciPjesme = new ObservableCollection<IzvodjacPjesma>();


			SearchCommand = new Command<string>(OnSearch);
			PlayPauseCommand = new Command(OnPlayPause);
			NextCommand = new Command(OnNext);
			PreviousCommand = new Command(OnPrevious);
			SelectSongCommand = new Command<Pjesma>(OnSongSelected);

			//LoadSongs();
			LoadSongsAsync();

			if (Songs.Any())
			{
				SelectedSong = Songs.First();
			}
		}

		/*private void LoadSongs()
		{
			Songs.Add(new Pjesma(
				naziv: "BASS",
				opis: "Jala Brat",
				putanjaAudio: "https://firebasestorage.googleapis.com/v0/b/trailerflix-25df2.appspot.com/o/items%2Ftest.mp3?alt=media&token=c7aa63d1-9c64-4660-af02-9c8809ed7c90",
				putanjaSlika: "https://i.scdn.co/image/ab67616d0000b273a8aa97fb6e61f7e092c166f6",
				jezikPjesme: "Bosanski",
				kreiranDatumVrijeme: DateTime.Now
			));
			Songs.Add(new Pjesma(
				naziv: "F1 theme",
				opis: "Brian Tyler",
				putanjaAudio: "https://firebasestorage.googleapis.com/v0/b/trailerflix-25df2.appspot.com/o/items%2FF1%20theme%20by%20Brian%20Tyler.mp3?alt=media&token=5de7f505-809e-48b4-8f3f-d249397afe1b",
				putanjaSlika: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQmBY8mn2_qmAmgCS6C3Ify_WWmVWmXOdpfsw&s",
				jezikPjesme: "Bosanski",
				kreiranDatumVrijeme: DateTime.Now
				));
		
		}*/

		private void OnSongSelected(Pjesma selectedSong)
		{
			SelectedSong = selectedSong;
		}

		private async Task LoadSongsAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var pjesme = JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					PjesmiceText = pjesme[0].naziv;
					Songs.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						Songs.Add(pjesma);
					}
					if (Songs.Any())
					{
						SelectedSong = Songs.First();
						CurrentSong = Songs.First();
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
		}

		private void LoadAlbums()
		{

		}

		private void LoadHistorijeSlusanja()
		{
		}

		private void LoadKorisnike()
		{
		}

		private void LoadKomentare()
		{
		}

		private void LoadPlayListe()
		{
		}

		private void LoadPretplate()
		{
		}

		private void LoadZanrove()
		{
		}

		private void LoadStatistikeReprodukcije()
		{
		}

		private void LoadPjesmeZanrove()
		{
		}

		private void LoadPjesmePlayListe()
		{
		}

		private void LoadPracenja()
		{
		}

		private void LoadObnovePretplate()
		{
		}

		private void LoadKorisniciPretplate()
		{
		}

		private void LoadKorisniciPlayListe()
		{
		}

		private void LoadKorisniciPjesme()
		{
		}

		private void LoadKorisniciAlbumi()
		{
		}

		private void LoadIzvodjacePjesme()
		{
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

		private void OnSongTapped(object sender, EventArgs e)
		{
			var tappedSong = (sender as TapGestureRecognizer)?.CommandParameter as Pjesma;
			if (tappedSong != null)
			{
				CurrentSong = tappedSong;
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
