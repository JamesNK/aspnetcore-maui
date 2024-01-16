using System.Diagnostics;
using System.IO.Pipelines;
using System.Security.Cryptography.X509Certificates;
using Android.App;
using Android.Runtime;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MauiApp;
using MauiApp.Web;

namespace MauiApp;

[Application(UsesCleartextTraffic = true)]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
    {
        var colorChanger = new ColorChanger();
        var loggerProvider = new CallbackLoggerProvider();

        _ = Task.Run(async () =>
        {
            try
            {
                var cert = await LoadCertificateAsync();

                var serverApp = await WebAppHostProgram.CreateWebApp(
                    httpPort: 5000,
                    httpsPort: 5001,
                    "MauiApp",
                    cert,
                    colorChanger,
                    loggerProvider);

                await serverApp.StartAsync();
            }
            catch (Exception ex)
            {
                Debug.Write("Error from startup.");
                Debug.Write(ex.ToString());
            }
        });

        var app = MauiProgram.CreateMauiApp(colorChanger, loggerProvider);

        return app;
    }

    private static async Task<X509Certificate2> LoadCertificateAsync()
    {
        using var pfxStream = await FileSystem.Current.OpenAppPackageFileAsync("aspnetdevcert.pfx");
        var pfx = new MemoryStream();
        await pfxStream.CopyToAsync(pfx);
        return new X509Certificate2(pfx.ToArray(), "testPassword");
    }
}
