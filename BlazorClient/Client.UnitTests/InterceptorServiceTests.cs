using Client.LocalStorage;
using Client.Services;
using Client.Services.Interfaces;
using Moq;
using Toolbelt.Blazor;

namespace Client.UnitTests;

public class InterceptorServiceTests
{
    [Fact]
    public void Intercept_On_Invoke_BeforeSendAsync_Success()
    {
        // arrange
        var mockInterceptor = new MockInterceptor();
        var refreshTokenMock = new Mock<IRefreshTokenService>();
        var wasInvoke = false;
        refreshTokenMock.Setup(x => x.RefreshTokenAsync()).Returns(() =>
        {
            wasInvoke = true;
            return Task.FromResult(false);
        });
        var localStorage = new Mock<ILocalStorageService>();
        var sut = new InterceptorService(mockInterceptor, refreshTokenMock.Object, localStorage.Object);

        // act
        sut.RegisterEvent();
        
        mockInterceptor.InvokeBeforeSendAsync();

        // assert
        Assert.True(wasInvoke);
    }
    
    private class MockInterceptor : IHttpClientInterceptor
    {
        public event EventHandler<HttpClientInterceptorEventArgs>? BeforeSend;
        public event HttpClientInterceptorEventHandler? BeforeSendAsync;
        public event EventHandler<HttpClientInterceptorEventArgs>? AfterSend;
        public event HttpClientInterceptorEventHandler? AfterSendAsync;

        public void InvokeBeforeSendAsync() => BeforeSendAsync?.Invoke(null, null);
    }
}