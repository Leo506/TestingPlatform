using System.Net.Http.Headers;
using System.Net.Http.Json;
using Client.LocalStorage;
using Client.Models;
using Client.Services.Interfaces;
using Client.ViewModels;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client.Services;

public class TestsService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly IInterceptorService _interceptorService;
    
    public TestsService(HttpClient httpClient, ILocalStorageService localStorage, IInterceptorService interceptorService)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _interceptorService = interceptorService;
        _interceptorService.RegisterOnEvents();
    }

    public async Task<IEnumerable<TestsModel>?> GetAllTests()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, "get/tests/all");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return new List<TestsModel>();
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);

        var response = await _httpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            return new List<TestsModel>();
        
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        var viewModels = await response.Content.ReadFromJsonAsync<TestViewModel[]>();

        var models = viewModels?.Select(x => x.ToTestModel());

        return models;
    }

    public async Task<bool> CreateTest(TestsModel model)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "create/test");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return false;
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);

        var viewModel = model.ToTestViewModel();
        viewModel.Id = "";
        
        message.Content = new StringContent(JsonSerializer.Serialize(viewModel));
        message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        

        var response = await _httpClient.SendAsync(message);

        if (response.IsSuccessStatusCode)
            return true;
        
        Console.WriteLine($"Create test status code: {response.StatusCode}");
        return false;
    }

    public async Task<bool> UpdateTest(TestsModel model)
    {
        var message = new HttpRequestMessage(HttpMethod.Put, $"update/test/{model.Id}");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return false;
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);

        var viewModel = model.ToTestViewModel();
        viewModel.Id = "";
        
        message.Content = new StringContent(JsonSerializer.Serialize(viewModel));
        message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        

        var response = await _httpClient.SendAsync(message);

        return response.IsSuccessStatusCode;
    }


    public async Task<bool> DeleteTest(string id)
    {
        var message = new HttpRequestMessage(HttpMethod.Delete, $"delete/test/{id}");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return false;
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);
        
        var response = await _httpClient.SendAsync(message);

        return response.IsSuccessStatusCode;
    }

    public async Task<TestsModel?> Get(string id)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"get/tests/{id}");
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return null;
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel?.AccessToken);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
            return null;

        var viewModel = await response.Content.ReadFromJsonAsync<TestViewModel>();

        return viewModel?.ToTestModel();
    }

    public void Dispose()
    {
        _interceptorService.Dispose();
    }
}