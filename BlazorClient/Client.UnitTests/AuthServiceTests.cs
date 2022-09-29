using System.Net;
using Client.LocalStorage;
using Client.Models;
using Client.Services;
using Client.UnitTests.Helpers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client.UnitTests;

public class AuthServiceTests
{
    [Fact]
    public async Task AuthenticateAsync_Success()
    {
        // arrange
        var localStorage = new Mock<ILocalStorageService>();
        var httpClient = new HttpClient(new MockHttpHandler());
        httpClient.BaseAddress = new Uri("https://localhost:5000");
        var sut = new AuthService(httpClient, localStorage.Object);

        // act
        var result = await sut.AuthenticateAsync("username", "password");


        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task AuthenticateAsync_CorrectMessage_Success()
    {
        // arrange
        const string expected = "grant_type=password&username=username&password=password";
        var localStorage = LocalStorageHelper.GetService();
        var handler = new MockHttpHandler();
        var httpClient = new HttpClient(handler);
        httpClient.BaseAddress = new Uri("https://localhost:5000");
        var sut = new AuthService(httpClient, localStorage);

        // act
        await sut.AuthenticateAsync("username", "password");
        var actual = handler.MessageContent;

        // assert
        Assert.Equal(expected, actual);
    }
    
    private class MockHttpHandler : HttpMessageHandler
    {
        public string MessageContent { get; private set; }

        public MockHttpHandler()
        {
            MessageContent = string.Empty;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = request.Content;
            MessageContent = await content?.ReadAsStringAsync(cancellationToken)!;

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new TokenModel()
                {
                    AccessToken = "11111",
                    RefreshToken = "11111",
                    ExpiresIn = 3600,
                    Scope = "scope",
                    TokenType = "type"
                }))
            };
        }
    }
}