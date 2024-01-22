using Android.App;
using Android.Runtime;

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
        return MauiProgram.CreateMauiApp();
    }
}
