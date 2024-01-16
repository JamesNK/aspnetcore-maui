using MauiApp.Web;
using Microsoft.Extensions.Logging;

namespace MauiApp;

public static class MauiProgram
{
    public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp(ColorChanger colorChanger, CallbackLoggerProvider callbackLoggerProvider)
    {
        var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(colorChanger);
        builder.Services.AddSingleton(callbackLoggerProvider);
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
