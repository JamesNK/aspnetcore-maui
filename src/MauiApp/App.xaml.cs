namespace MauiApp;

public partial class App : Application
{
    public App(WebAppHost webAppHost)
    {
        InitializeComponent();

        MainPage = new AppShell();

        // Start web app server.
        _ = Task.Run(() => webAppHost.StartAsync());
    }
}
