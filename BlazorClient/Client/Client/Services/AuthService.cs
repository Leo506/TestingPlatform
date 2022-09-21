using System.Net.Http.Json;
using Client.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TokenModel?> AuthenticateAsync(string username, string password)
    {
        var messageDict = new Dictionary<string, string>()
        {
            ["grant_type"] = "password",
            ["username"] = username,
            ["password"] = password
        };

        var message = new HttpRequestMessage(new HttpMethod("post"), "connect/token");
        message.Content = new FormUrlEncodedContent(messageDict);
        //message.SetBrowserRequestMode(BrowserRequestMode.NoCors);

        var response = await _httpClient.SendAsync(message);

        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Auth response: " + responseString);
        Console.WriteLine("Url: " + response.RequestMessage?.RequestUri);
        Console.WriteLine("Status code: " + response.StatusCode);

        if (!response.IsSuccessStatusCode) return null;
        
        var tokenModel = await response.Content.ReadFromJsonAsync<TokenModel>();

        return tokenModel;

    }
}