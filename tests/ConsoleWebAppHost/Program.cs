using System.Security.Cryptography.X509Certificates;
using MauiApp.Web;

var cert = new X509Certificate2("aspnetdevcert.pfx", "testPassword");

var colorChanger = new ColorChanger();
var loggerProvider = new CallbackLoggerProvider();

var serverApp = await WebAppHostProgram.CreateWebApp(
    httpPort: 5010,
    httpsPort: 5011,
    "ConsoleWebAppHost",
    cert,
    colorChanger,
    loggerProvider);

await serverApp.RunAsync();