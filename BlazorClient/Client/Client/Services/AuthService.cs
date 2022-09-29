using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Client.LocalStorage;
using Client.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var messageDict = new Dictionary<string, string>()
        {
            ["grant_type"] = "password",
            ["username"] = username,
            ["password"] = password
        };

        var message = new HttpRequestMessage(new HttpMethod("post"), "connect/token");
        message.Content = new FormUrlEncodedContent(messageDict);

        try
        {
            var response = await _httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode) return false;
        
            var tokenModel = await response.Content.ReadFromJsonAsync<TokenModel>();
            if (tokenModel is null)
                return false;
            
            Console.WriteLine(tokenModel.AccessToken);
            
            tokenModel.Issued = DateTime.UtcNow;

            await _localStorage.SetAsync(nameof(TokenModel), tokenModel);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}