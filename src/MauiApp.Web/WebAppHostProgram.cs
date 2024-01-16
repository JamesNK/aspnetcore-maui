using System.Net;
using System.Security.Cryptography.X509Certificates;
using MauiApp.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MauiApp.Web;

public class WebAppHostProgram
{
    public static async Task<WebApplication> CreateWebApp(int httpPort, int httpsPort, string applicationName, X509Certificate2 certificate, ColorChanger colorChanger, CallbackLoggerProvider loggerProvider)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = applicationName
        });
        builder.WebHost.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.Listen(IPAddress.Loopback, httpPort);
            serverOptions.Listen(IPAddress.Loopback, httpsPort, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                listenOptions.UseHttps(certificate);
            });
        });

        // SignalR
        builder.Services.AddSignalR();

        builder.Services.AddMvc().AddApplicationPart(typeof(WebAppHostProgram).Assembly);

        // Auth
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));
        builder.Services.AddIdentityCore<MyUser>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddApiEndpoints();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Logging.AddProvider(loggerProvider);
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Services.AddSingleton(colorChanger);

        var app = builder.Build();

        app.MapHub<ChatHub>("/chathub");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        var api = app.MapGroup("/api");
        api.MapIdentityApi<MyUser>();
        api.MapClientApi();

        //app.UseWelcomePage();

        app.MapGet("/", () => "Hello World!");
        app.MapControllers();

        await Auth.InitializeTestUserAsync(app.Services);

        return app;
    }
}
