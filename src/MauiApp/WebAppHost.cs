using System.Security.Cryptography.X509Certificates;
using MauiApp.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace MauiApp;

public class WebAppHost : IAsyncDisposable
{
    private readonly ILogger<WebAppHost> _logger;
    private readonly MessageDispatcher _colorChanger;
    private readonly CallbackLoggerProvider _loggerProvider;
    private WebApplication? _app;

    public WebAppHost(ILogger<WebAppHost> logger, MessageDispatcher messageDispatcher, CallbackLoggerProvider loggerProvider)
    {
        _logger = logger;
        _colorChanger = messageDispatcher;
        _loggerProvider = loggerProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting web app.");

        try
        {
            var cert = await LoadCertificateAsync();

            _app = await WebAppHostProgram.CreateWebApp(
                httpPort: 5000,
                httpsPort: 5001,
                "MauiApp",
                cert,
                _colorChanger,
                _loggerProvider);

            await _app.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error from web app startup.");
        }
    }

    private static async Task<X509Certificate2> LoadCertificateAsync()
    {
        using var pfxStream = await FileSystem.Current.OpenAppPackageFileAsync("aspnetdevcert.pfx");
        var pfx = new MemoryStream();
        await pfxStream.CopyToAsync(pfx);
        return new X509Certificate2(pfx.ToArray(), "testPassword");
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Stopping web app.");

        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
