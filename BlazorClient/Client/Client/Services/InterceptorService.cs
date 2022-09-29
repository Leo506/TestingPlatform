using System.Net.Http.Headers;
using Client.LocalStorage;
using Client.Models;
using Client.Services.Interfaces;
using Toolbelt.Blazor;

namespace Client.Services;

public class InterceptorService : IInterceptorService
{
    private readonly IHttpClientInterceptor _interceptor;

    private readonly IRefreshTokenService _refreshTokenService;

    private readonly ILocalStorageService _localStorageService;

    public InterceptorService(IHttpClientInterceptor interceptor, IRefreshTokenService refreshTokenService,
        ILocalStorageService localStorageService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
        _localStorageService = localStorageService;
    }

    public void Dispose()
    {
        _interceptor.BeforeSendAsync -= OnBeforeSendAsync;
    }

    public void RegisterOnEvents()
    {
        _interceptor.BeforeSendAsync += OnBeforeSendAsync;
    }

    public Task OnBeforeSend(object sender, HttpClientInterceptorEventArgs e)
    {
        throw new NotImplementedException();
    }

    public async Task OnBeforeSendAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var refresh = await _refreshTokenService.RefreshTokenAsync();
        
        if (!refresh)
            return;

        var token = await _localStorageService.GetAsync<TokenModel>(nameof(TokenModel));
        e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
    }

    public Task OnAfterSend(object sender, HttpClientInterceptorEventArgs e)
    {
        throw new NotImplementedException();
    }

    public Task OnAfterSendAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        throw new NotImplementedException();
    }
}