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

public class TestsService : RemoteServiceBase, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IInterceptorService _interceptorService;

    public TestsService(HttpClient httpClient, ILocalStorageService localStorage,
        IInterceptorService interceptorService) : base(localStorage)
    {
        _httpClient = httpClient;
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
}