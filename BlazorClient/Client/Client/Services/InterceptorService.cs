using System.Net.Http.Headers;
using Client.LocalStorage;
using Client.Models;
using Toolbelt.Blazor;

namespace Client.Services;

public class InterceptorService
{
    private readonly HttpClientInterceptor _interceptor;

    private readonly RefreshTokenService _refreshTokenService;

    private readonly ILocalStorageService _localStorageService;

    public InterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService,
        ILocalStorageService localStorageService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
        _localStorageService = localStorageService;
    }

    public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptorOnBeforeSendAsync;

    private async Task InterceptorOnBeforeSendAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var refresh = await _refreshTokenService.RefreshToken();
        
        if (!refresh)
            return;

        var token = await _localStorageService.GetAsync<TokenModel>(nameof(TokenModel));
        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptorOnBeforeSendAsync;
}