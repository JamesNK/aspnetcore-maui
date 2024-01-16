using Microsoft.AspNetCore.SignalR.Client;

await using var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5000/ChatHub")
    .WithAutomaticReconnect(new RetryPolicy())
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

Console.WriteLine("Connecting to phone...");

await connection.StartAsync();

Console.WriteLine("Connected.");

while (true)
{
    Console.WriteLine("Enter color or exit:");
    var content = Console.ReadLine();
    await connection.SendAsync("SendMessage", "ConsoleApp", content);

    if (string.Equals(content, "exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }
}

Console.WriteLine("Exiting...");
