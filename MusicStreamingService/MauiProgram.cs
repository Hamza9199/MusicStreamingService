using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using The49.Maui.BottomSheet;
using MusicStreamingService.Services;
using MusicStreamingService.Views;

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

            builder.Services.AddSingleton<ILoginRepository, LoginService>();
			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<Login>();
			builder.Services.AddSingleton<Register>();
			builder.Services.AddSingleton<Postavke>();
#if DEBUG
			builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}