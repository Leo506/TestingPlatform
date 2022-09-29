using System.Net.Http.Json;
using Client.LocalStorage;
using Client.Models;
using Client.Services.Interfaces;

namespace Client.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;
    
    public RefreshTokenService(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        var tokenModel = await _localStorageService.GetAsync<TokenModel>(nameof(TokenModel));

        if (tokenModel is null)
        {
            Console.WriteLine("Token model is null, but need to refresh");
            return false;
        }
        
        var exp = DateTime.UtcNow.AddSeconds(tokenModel.ExpiresIn);
        var now = DateTime.UtcNow;

        if ((exp - now).TotalMinutes < 2)
        {
            var messageDict = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", tokenModel.RefreshToken }
            };

            var message = new HttpRequestMessage(HttpMethod.Post, "connect/token");
            message.Content = new FormUrlEncodedContent(messageDict);

            var response = await _httpClient.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                var newTokenModel = await response.Content.ReadFromJsonAsync<TokenModel>();
                if (newTokenModel is null)
                {
                    Console.WriteLine("Error while deserializing refresh token response");
                    return false;
                }
                
                await _localStorageService.SetAsync<TokenModel>(nameof(TokenModel), newTokenModel);
            }

            return true;
        }

        return false;
    }
}