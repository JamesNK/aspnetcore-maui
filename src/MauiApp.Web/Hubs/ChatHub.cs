using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MauiApp.Web.Hubs;

internal class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly ColorChanger _colorChanger;

    public ChatHub(ILogger<ChatHub> logger, ColorChanger colorChanger)
    {
        _logger = logger;
        _colorChanger = colorChanger;
    }

    public async Task SendMessage(string user, string message)
    {
        _logger.LogInformation("Message received: {Message}", message);

        _colorChanger.ChangeColor(message);

        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
