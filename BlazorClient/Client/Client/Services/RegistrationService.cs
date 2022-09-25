using System.Net.Http.Headers;
using System.Text.Json;
using Client.Pages;

namespace Client.Services;

public class RegistrationService
{
    private readonly HttpClient _client;

    public RegistrationService(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> RegisterAsync(RegistrationViewModel viewModel)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, $"/connect/registration");
        message.Content = new StringContent(JsonSerializer.Serialize(viewModel));
        message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        var response = await _client.SendAsync(message);

        return response.IsSuccessStatusCode;
    }
}