using System.Net.Http.Headers;
using System.Text.Json;
using Calabonga.OperationResults;
using Client.LocalStorage;
using Client.Models;
namespace Client.Services;

public class RemoteServiceBase
{
    protected readonly ILocalStorageService LocalStorageService;

    public RemoteServiceBase(ILocalStorageService localStorageService)
    {
        LocalStorageService = localStorageService;
    }

    protected async Task<OperationResult<HttpRequestMessage>> CreateMessage(HttpMethod method, string url)
    {
        var result = OperationResult.CreateResult<HttpRequestMessage>();
        var message = new HttpRequestMessage(method, url);
        var tokenResult = await TryGetToken();
        if (!tokenResult.Ok)
        {
            result.AddError(tokenResult.Exception?.Message);
            return result;
        }
        message.Headers.Authorization = CreateAuthHeader(tokenResult.Result!);

        result.Result = message;

        return result;
    }

    protected async Task<OperationResult<HttpRequestMessage>> CreateMessage<T>(HttpMethod method, string url, T value)
    where T : class
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

        result.Result = await LocalStorageService.GetAsync<TokenModel>(nameof(TokenModel));

        if (result.Result is null)
            result.AddError("Not token data");

        return result;
    }

    private AuthenticationHeaderValue CreateAuthHeader(TokenModel token) =>
        new AuthenticationHeaderValue("Bearer", token.AccessToken);
}