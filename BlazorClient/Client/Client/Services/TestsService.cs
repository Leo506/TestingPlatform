using System.Net.Http.Headers;
using System.Net.Http.Json;
using Calabonga.OperationResults;
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
        var messageResult = await CreateMessage(HttpMethod.Get, "get/tests/all");
        if (!messageResult.Ok)
            return new List<TestsModel>();

        var response = await _httpClient.SendAsync(messageResult.Result!);
        if (!response.IsSuccessStatusCode)
            return new List<TestsModel>();
        
        var viewModels = await response.Content.ReadFromJsonAsync<TestViewModel[]>();

        var models = viewModels?.Select(x => x.ToTestModel());

        return models;
    }

    public async Task<bool> CreateTest(TestsModel model)
    {
        var viewModel = model.ToTestViewModel();
        viewModel.Id = "";
        var messageResult = await CreateMessage<TestViewModel>(HttpMethod.Post, "create/test", viewModel);
        if (!messageResult.Ok)
            return false;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateTest(TestsModel model)
    {
        var viewModel = model.ToTestViewModel();
        viewModel.Id = "";

        var messageResult = await CreateMessage<TestViewModel>(HttpMethod.Post, $"update/test/{model.Id}", viewModel);
        if (!messageResult.Ok)
            return false;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        return response.IsSuccessStatusCode;
    }


    public async Task<bool> DeleteTest(string id)
    {
        var messageResult = await CreateMessage(HttpMethod.Delete, $"delete/test/{id}");
        if (!messageResult.Ok)
            return false;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        return response.IsSuccessStatusCode;
    }

    public async Task<TestsModel?> Get(string id)
    {
        var messageResult = await CreateMessage(HttpMethod.Get, $"get/tests/{id}");
        if (!messageResult.Ok)
            return null;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        if (!response.IsSuccessStatusCode)
            return null;

        var viewModel = await response.Content.ReadFromJsonAsync<TestViewModel>();

        return viewModel?.ToTestModel();
    }

    public void Dispose() => _interceptorService.Dispose();

    private async Task<OperationResult<HttpRequestMessage>> CreateMessage(HttpMethod method, string url)
    {
        var result = OperationResult.CreateResult<HttpRequestMessage>();
        var message = new HttpRequestMessage(method, url);
        var tokenResult = await TryGetToken();
        if (!tokenResult.Ok)
        {
            result.AddError(tokenResult?.Exception?.Message);
            return result;
        }
        message.Headers.Authorization = CreateAuthHeader(tokenResult.Result!);

        result.Result = message;

        return result;
    }

    private async Task<OperationResult<HttpRequestMessage>> CreateMessage<T>(HttpMethod method, string url, T value)
    {
        var result = OperationResult.CreateResult<HttpRequestMessage>();
        var message = new HttpRequestMessage(method, url);
        var tokenResult = await TryGetToken();
        if (!tokenResult.Ok)
        {
            result.AddError(tokenResult?.Exception?.Message);
            return result;
        }
        message.Headers.Authorization = CreateAuthHeader(tokenResult.Result!);
        
        message.Content = new StringContent(JsonSerializer.Serialize(value));
        message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        result.Result = message;

        return result;
    }

    private async Task<OperationResult<TokenModel>> TryGetToken()
    {
        var result = OperationResult.CreateResult<TokenModel>();

        result.Result = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));

        if (result.Result is null)
            result.AddError("Not token data");

        return result;
    }

    private AuthenticationHeaderValue CreateAuthHeader(TokenModel token) =>
        new AuthenticationHeaderValue("Bearer", token.AccessToken);
}