using MauiApp.Web;
using Microsoft.Extensions.Logging;

namespace MauiApp;

public partial class MainPage : ContentPage
{
    public MainPage(ColorChanger colorChanger, CallbackLoggerProvider callbackLoggerProvider)
    {
        InitializeComponent();

        colorChanger.Register(colorName =>
        {
            if (string.Equals(colorName, "exit", StringComparison.OrdinalIgnoreCase))
            {
                _ = Dispatcher.DispatchAsync(async () =>
                {
                    await Task.Delay(1000);
                    await WriteToOutput($"Shutting down in 3 seconds...");
                    await Task.Delay(3000);
                    Application.Current?.Quit();
                    System.Environment.Exit(0);
                });
                return;
            }

            var color = (Color?)typeof(Colors).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(x => x.FieldType == typeof(Color))
                .FirstOrDefault(x => string.Equals(x.Name, colorName, StringComparison.OrdinalIgnoreCase))
                ?.GetValue(null);

            if (color != null)
            {
                Dispatcher.Dispatch(() => ContentPage.BackgroundColor = color);
            }
        });

        callbackLoggerProvider.Register(record =>
        {
            _ = Dispatcher.DispatchAsync(async () =>
            {
                var text = $"{GetLogLevelString(record.LogLevel)}: {record.Message}";
                if (record.Exception != null)
                {
                    text += $"{Environment.NewLine}{record.Exception}";
                }
                await WriteToOutput(text);
            });                
        });
    }

    private async Task WriteToOutput(string message)
    {
        ServerLogOutput.Text += $"{((ServerLogOutput.Text?.Length > 0) ? Environment.NewLine : string.Empty)}{message}";
        await Task.Delay(100);
        await CounterScrollView.ScrollToAsync(ServerLogOutput, ScrollToPosition.End, false);
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}
