using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace MusicStreamingService
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");

				fonts.AddFont("Brands-Regular-400.ttf", "FAB");
				fonts.AddFont("Free-Regular-400.ttf", "FAR");
				fonts.AddFont("Free-Solid-900.ttf", "FAS");
			}).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}