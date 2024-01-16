using System.Net.Http.Json;
using System.Text.Json.Nodes;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5000");

var email = "test@contoso.com";
var password = "Password1!";

var response = await client.PostAsJsonAsync("/api/login", new { email = email, password = password }, CancellationToken.None);
response.EnsureSuccessStatusCode();

var content = await response.Content.ReadFromJsonAsync<JsonObject>(CancellationToken.None);

var token = (string)content!["accessToken"]!;

Console.WriteLine(content);
Console.WriteLine();
Console.WriteLine(token);

var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/weather");
httpRequestMessage.Headers.Add("Authorization", $"Bearer {token}");

var weatherResponse = await client.SendAsync(httpRequestMessage);
var weatherForecasts = await weatherResponse.Content.ReadFromJsonAsync<List<WeatherForecast>>(CancellationToken.None);

Console.WriteLine($"Weather forecasts: {weatherForecasts!.Count}");
