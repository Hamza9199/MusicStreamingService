using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using The49.Maui.BottomSheet;

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
                .UseBottomSheet()
				.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");

				fonts.AddFont("Brands-Regular-400.ttf", "FAB");
				fonts.AddFont("Free-Regular-400.ttf", "FAR");
				fonts.AddFont("Free-Solid-900.ttf", "FAS");
			}).UseMauiCommunityToolkit().UseBottomSheet();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}