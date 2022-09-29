using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace Client.UnitTests.Helpers;

public static class HttpClientHelper
{
    public static HttpClient GetSimpleClient()
    {
        return new HttpClient()
        {
            BaseAddress = new Uri("https://test")
        };
    }

    public static HttpClient GetBadClient()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        return new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://test")
        };
    }

    public static HttpClient GetClient<T>(T contentValue)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(contentValue))
            });
        
        return new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://test")
        };
    }
}