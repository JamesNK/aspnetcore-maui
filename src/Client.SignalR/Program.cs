using Microsoft.AspNetCore.SignalR.Client;

var tcs = new TaskCompletionSource();

await using var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/ChatHub")
    .WithAutomaticReconnect(new RetryPolicy())
    .Build();

connection.On<string>("ReceiveMessage", async (message) =>
{
    if (string.Equals(message, "exit", StringComparison.OrdinalIgnoreCase))
    {
        tcs.TrySetResult();
        return;
    }

    Console.WriteLine($"Received: {message}");

    Console.WriteLine("Enter color or exit:");
    while (true)
    {
        var content = Console.ReadLine();
        if (!string.IsNullOrEmpty(content))
        {
            await connection.SendAsync("SendMessage", content);
            break;
        }
    }
});

Console.WriteLine("Connecting to device...");

await connection.StartAsync();

await tcs.Task;

Console.WriteLine("Exiting...");
await Task.Delay(500);
