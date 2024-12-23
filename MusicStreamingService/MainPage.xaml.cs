namespace MusicStreamingService;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnLogoutClicked(object sender, EventArgs e)
	{
		Navigation.PopAsync();
	}

	private void OnPlaySongClicked(object sender, EventArgs e)
	{
		string? youtubeUrl = YouTubeLinkEntry.Text?.Trim();

		if (string.IsNullOrEmpty(youtubeUrl) || !youtubeUrl.Contains("youtube.com/watch"))
		{
			DisplayAlert("Error", "Please enter a valid YouTube link.", "OK");
			return;
		}

		var videoId = GetYouTubeVideoId(youtubeUrl);
		if (!string.IsNullOrEmpty(videoId))
		{
			string embedUrl = $"https://www.youtube.com/embed/{videoId}?autoplay=1";
			YouTubeWebView.Source = embedUrl;
			YouTubeWebView.IsVisible = true;
		}
		else
		{
			DisplayAlert("Error", "Invalid YouTube link.", "OK");
		}
	}

	private string GetYouTubeVideoId(string youtubeUrl)
	{
		var uri = new Uri(youtubeUrl);
		var query = uri.Query;
		var queryParams = System.Web.HttpUtility.ParseQueryString(query);
		return queryParams["v"];
	}
}
