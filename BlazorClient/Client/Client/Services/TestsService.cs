using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.LocalStorage;
using Client.Models;
using Client.ViewModels;

namespace Client.Services;

public class TestsService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    
    public TestsService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<IEnumerable<TestsModel>?> GetAllTests()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, "get/tests/all");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);

        var response = await _httpClient.SendAsync(message);
        
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        var viewModels = await response.Content.ReadFromJsonAsync<TestViewModel[]>();

        var models = viewModels?.Select(x => x.ToTestModel());

        return models;
    }
}