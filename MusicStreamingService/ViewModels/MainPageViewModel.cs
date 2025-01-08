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
using MusicStreamingService.Views;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace MusicStreamingService.ViewModels
{
	public class MainPageViewModel : INotifyPropertyChanged
	{

		private readonly HttpClient _httpClient;

		public ObservableCollection<Pjesma> Songs { get; set; } 

		public ObservableCollection<Models.Album> Albumi { get; set; }

		public ObservableCollection<HistorijaSlusanja> historijeSlusanja { get; set; }

		public ObservableCollection<Models.DobiveniKorisnik> korisnici { get; set; }

		public ObservableCollection<Komentar> komentari { get; set; }

		public ObservableCollection<Models.PlayLista> playListe { get; set; }

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

		public Pjesma pjesma1 { get; set; }

		public Models.Album pjesma2 { get; set; }

		public Models.PlayLista pjesma3 { get; set; }

		//public ICommand SearchCommand { get; } 
		//public ICommand PlayPauseCommand { get; }
		//public ICommand NextCommand { get; } 
		//public ICommand PreviousCommand { get; }

		public ICommand SelectSongCommand { get; }
		public ICommand SelectAlbumCommand { get; }
		public ICommand SelectPlaylistaCommand { get; }
		public ICommand SelectKorisnikCommand { get; }

		public ICommand OpenSomethingCommand1 { get; }

		public ICommand OpenSomethingCommand2 { get; }

		public ICommand OpenSomethingCommand3 { get; }

		public ICommand OpenSomethingCommand4{ get; }




		//public string PlayPauseButtonText => IsPlaying ? "⏸️" : "▶️"; 

		private Pjesma _currentSong;

		private Models.Album _currentAlbum;

		private Models.PlayLista _currentPlayLista;

		private Models.DobiveniKorisnik _currentKorisnik;




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

		public Models.Album CurrentAlbum
		{
			get => _currentAlbum;
			set
			{
				_currentAlbum = value;
				OnPropertyChanged();
			}
		}

		public Models.PlayLista CurrentPlaylista
		{
			get => _currentPlayLista;
			set
			{
				_currentPlayLista = value;
				OnPropertyChanged();
			}
		}

		public Models.DobiveniKorisnik CurrentKorisnik
		{
			get => _currentKorisnik;
			set
			{
				_currentKorisnik = value;
				OnPropertyChanged();
			}
		}

		private bool isLoading;
		public bool IsLoading
		{
			get => isLoading;
			set
			{
				isLoading = value;
				OnPropertyChanged(nameof(IsLoading));
			}
		}


		/*private bool _isPlaying;
		public bool IsPlaying
		{
			get => _isPlaying;
			set
			{
				_isPlaying = value;
				OnPropertyChanged();
			}
		}*/
		public MainPageViewModel()
		{
			CrossMediaManager.Current.Init();

			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("http://risdecibeltest-001-site1.otempurl.com/") 
			};

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("11205261:60-dayfreetrial")));

			Songs = new ObservableCollection<Pjesma>();
			Albumi = new ObservableCollection<Models.Album>();
			historijeSlusanja = new ObservableCollection<HistorijaSlusanja>();
			korisnici = new ObservableCollection<DobiveniKorisnik>();
			komentari = new ObservableCollection<Komentar>();
			playListe = new ObservableCollection<Models.PlayLista>();
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


			//SearchCommand = new Command<string>(OnSearch);
			/*PlayPauseCommand = new Command(OnPlayPause);
			NextCommand = new Command(OnNext);
			PreviousCommand = new Command(OnPrevious);*/
			pjesma1 = new Pjesma();
			pjesma2 = new Models.Album();
			pjesma3 = new Models.PlayLista();

			SelectSongCommand = new Command(OnSongSelected);
			SelectAlbumCommand = new Command(OnAlbumSelected);
			SelectPlaylistaCommand = new Command(OnPlayListaSelected);
			SelectKorisnikCommand = new Command(OnKorisnikSelected);
			OpenSomethingCommand1 = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new AudioPlayer(pjesma1)));
			OpenSomethingCommand2 = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new Views.Album(pjesma2)));
			OpenSomethingCommand3 = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new Views.Playlista(pjesma3)));
			OpenSomethingCommand4 = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new LajkovanePjesme()));

			

			//LoadSongs();
			_ = LoadSongsAsync();
			_ = LoadAlbums();
			_ = LoadKorisnike();
			_ = LoadPlayListe();


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

		

		private async void OnSongSelected()
		{
			if (IsLoading)
				return;

			if (CurrentSong != null) 
			{
				await Application.Current.MainPage.Navigation.PushAsync(new AudioPlayer(CurrentSong));

			}

		}

		private async void OnAlbumSelected()
		{
			if (IsLoading)
				return;
			if (CurrentAlbum != null)
			{
				await Application.Current.MainPage.Navigation.PushAsync(new Views.Album(CurrentAlbum));
			}
		}

		private async void OnPlayListaSelected()
		{
			if (IsLoading)
				return;
			if (CurrentPlaylista != null)
			{
				await Application.Current.MainPage.Navigation.PushAsync(new Views.Playlista(CurrentPlaylista));
			}
		}

		private async void OnKorisnikSelected()
		{
			if (IsLoading)
				return;
			if (CurrentKorisnik != null)
			{
				await Application.Current.MainPage.Navigation.PushAsync(new PregledIzvodaca(CurrentKorisnik));
			}
		}

		private async Task LoadSongsAsync()
		{	
			try
			{
				isLoading = true;
				var response = await _httpClient.GetAsync("api/PjesmaControllerAPI");
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var pjesme = System.Text.Json.JsonSerializer.Deserialize<List<Pjesma>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (pjesme != null)
				{
					Songs.Clear();

					foreach (var pjesma in pjesme)
					{
						Debug.WriteLine($"Naziv: {pjesma.naziv}, Opis: {pjesma.opis}");
						Songs.Add(pjesma);
					}
					var random = new Random();
					var uniqueIndexes = new HashSet<int>();
					while (uniqueIndexes.Count < 3 && uniqueIndexes.Count < Songs.Count)
					{
						uniqueIndexes.Add(random.Next(Songs.Count));
					}

					var selectedIndexes = uniqueIndexes.ToList();
					pjesma1 = Songs[0];
					

					Debug.WriteLine(pjesma1);
					foreach (var prop in pjesma1.GetType().GetProperties())
					{
						Debug.WriteLine(prop.Name + ": " + prop.GetValue(pjesma1));
					}
					Debug.WriteLine(pjesma2);
					foreach (var prop in pjesma2.GetType().GetProperties())
					{
						Debug.WriteLine(prop.Name + ": " + prop.GetValue(pjesma2));
					}

					Debug.WriteLine(pjesma3);
					foreach (var prop in pjesma3.GetType().GetProperties())
					{
						Debug.WriteLine(prop.Name + ": " + prop.GetValue(pjesma3));
					}
					OnPropertyChanged(nameof(pjesma1));
					OnPropertyChanged(nameof(pjesma2));
					OnPropertyChanged(nameof(pjesma3));

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja pjesama: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}
		}

		private async Task LoadAlbums()
		{

			try
			{
				var response = await _httpClient.GetAsync("api/AlbumControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var albumi = System.Text.Json.JsonSerializer.Deserialize<List<Models.Album>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (albumi != null)
				{
					//Albumi.Clear();
					foreach (var album in albumi)
					{
						Debug.WriteLine($"Naziv: {album.naziv}, Opis: {album.opis}");
						Albumi.Add(album);
					}
					var random = new Random();
					var uniqueIndexes = new HashSet<int>();
					while (uniqueIndexes.Count < 3 && uniqueIndexes.Count < Albumi.Count)
					{
						uniqueIndexes.Add(random.Next(Albumi.Count));
					}

					var selectedIndexes = uniqueIndexes.ToList();
					pjesma2 = Albumi[1];
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja albuma: {ex.Message}");

			}
		}

		private async Task LoadHistorijeSlusanja()
		{

		}

		private async Task LoadKorisnike()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/KorisnikControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");

				var korisnici2 = System.Text.Json.JsonSerializer.Deserialize<List<DobiveniKorisnik>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				if (korisnici2 != null && korisnici2.Any())
				{
					foreach (var korisnik in korisnici2)
					{
						Debug.WriteLine($"Naziv: {korisnik.Ime}, Opis: {korisnik.Prezime}, id { korisnik.Id}");
						korisnici.Add(korisnik);
					}
				}
				else
				{
					Debug.WriteLine("Lista korisnika je prazna ili null.");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Greška prilikom učitavanja korisnika: {ex.Message}");
			}
		}


		private async Task LoadKomentare()
		{
		}

		private async Task LoadPlayListe()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/PlaylistaControllerAPI");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				Debug.WriteLine($"Response content: {json}");
				var playListe2 = System.Text.Json.JsonSerializer.Deserialize<List<Models.PlayLista>>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				if (playListe2 != null)
				{
					//playListe.Clear();
					foreach (var playLista in playListe2)
					{
						Debug.WriteLine($"Naziv: {playLista.naziv}, Opis: {playLista.opis}");
						playListe.Add(playLista);
					}
					var random = new Random();
					var uniqueIndexes = new HashSet<int>();
					while (uniqueIndexes.Count < 3 && uniqueIndexes.Count < playListe.Count)
					{
						uniqueIndexes.Add(random.Next(playListe.Count));
					}

					var selectedIndexes = uniqueIndexes.ToList();
					pjesma3 = playListe[0];

				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Greška prilikom učitavanja play liste: {ex.Message}");
			}
		}

		private async Task LoadPretplate()
		{
		}

		private async Task LoadZanrove()
		{
		}

		private async Task LoadStatistikeReprodukcije()
		{
		}

		private async Task LoadPjesmeZanrove()
		{
		}

		private async Task LoadPjesmePlayListe()
		{
		}

		private async Task LoadPracenja()
		{
		}

		private async Task LoadObnovePretplate()
		{
		}

		private async Task LoadKorisniciPretplate()
		{
		}

		private async Task LoadKorisniciPlayListe()
		{
		}

		private async Task LoadKorisniciPjesme()
		{
		}

		private async Task LoadKorisniciAlbumi()
		{
		}

		private async Task LoadIzvodjacePjesme()
		{
		}




		/*private void OnSearch(string query)
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
		}*/

		/*private string GetAudioPath(Pjesma pjesma)
		{
			if (pjesma == null)
			{
				System.Diagnostics.Debug.WriteLine("Greška: Nije selektovana nijedna pjesma.");
				return string.Empty;
			}

			return pjesma.putanjaAudio;
		}*/

		/*private async void OnPlayPause()
		{
			if (CurrentSong == null)
			{
				await App.Current.MainPage.DisplayAlert("Greška", "Nije odabrana nijedna pjesma.", "U redu");
				return;
			}

			var mediaManager = CrossMediaManager.Current;

			if (IsPlaying)
			{
				await mediaManager.Pause();
				IsPlaying = false;
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



					IsPlaying = true;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Greška prilikom reprodukcije: {ex.Message}");
					await App.Current.MainPage.DisplayAlert("Greška", "Ne mogu reproducirati pjesmu.", "U redu");
				}
			}

			OnPropertyChanged(nameof(PlayPauseButtonText));
		}*/

		/*private void OnNext()
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
		}*/

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
