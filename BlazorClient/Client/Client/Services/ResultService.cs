using Client.LocalStorage;
using Client.Models;

namespace Client.Services;

public class ResultService : RemoteServiceBase
{
    private readonly HttpClient _httpClient;
    public ResultService(ILocalStorageService localStorageService, HttpClient httpClient) : base(localStorageService)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> SendResultAsync(ResultModel result)
    {
        var messageResult = await CreateMessage(HttpMethod.Post, "/result/new/");
        if (!messageResult.Ok)
            return false;

        var response = await _httpClient.SendAsync(messageResult.Result!);

        return response.IsSuccessStatusCode;
    }
}