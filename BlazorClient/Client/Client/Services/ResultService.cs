using Client.LocalStorage;
using Client.Models;
using Client.Services.Interfaces;

namespace Client.Services;

public class ResultService : RemoteServiceBase, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IInterceptorService _interceptorService;

    public ResultService(ILocalStorageService localStorageService, HttpClient httpClient,
        IInterceptorService interceptorService) : base(localStorageService)
    {
        _httpClient = httpClient;
        _interceptorService = interceptorService;
        _interceptorService.RegisterOnEvents();
    }

    public async Task<bool> SendResultAsync(ResultModel result)
    {
        var messageResult = await CreateMessage<ResultModel>(HttpMethod.Post, "result/new", result);
        if (!messageResult.Ok)
            return false;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        return response.IsSuccessStatusCode;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        _interceptorService.Dispose();
    }
}