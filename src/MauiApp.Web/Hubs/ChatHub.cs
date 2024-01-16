using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MauiApp.Web.Hubs;

internal class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly MessageDispatcher _messageDispatcher;

    public ChatHub(ILogger<ChatHub> logger, MessageDispatcher messageDispatcher)
    {
        _logger = logger;
        _messageDispatcher = messageDispatcher;
    }

    public async Task SendMessage(string message)
    {
        _logger.LogInformation("Message received: {Message}", message);

        _messageDispatcher.DispatchMessage(message);

        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"Connected to {RuntimeInformation.RuntimeIdentifier}.");
    }
}
