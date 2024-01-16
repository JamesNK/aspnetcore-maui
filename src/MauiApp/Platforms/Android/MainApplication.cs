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
        var app = MauiProgram.CreateMauiApp();

        return app;
    }
}
