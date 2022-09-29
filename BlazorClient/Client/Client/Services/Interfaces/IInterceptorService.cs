using Toolbelt.Blazor;

namespace Client.Services.Interfaces;

public interface IInterceptorService : IDisposable
{
    void RegisterOnEvents();
    Task OnBeforeSend(object sender, HttpClientInterceptorEventArgs e);
    Task OnBeforeSendAsync(object sender, HttpClientInterceptorEventArgs e);
    Task OnAfterSend(object sender, HttpClientInterceptorEventArgs e);
    Task OnAfterSendAsync(object sender, HttpClientInterceptorEventArgs e);
}