using System.Net;
using System.Text.Json;
using Client.LocalStorage;
using Client.Models;
using Client.Services;
using Client.UnitTests.Helpers;
using Moq;
using Moq.Protected;

namespace Client.UnitTests;

public class RefreshTokenServiceTests
{
    [Fact]
    public async Task RefreshToken_No_TokenModel_Returns_False()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(null);
        
        
        var sut = new RefreshTokenService(localStorage, new HttpClient());

        // act
        var result = await sut.RefreshTokenAsync();

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task RefreshToken_Token_Expires_Less_2_Minutes_Return_True()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(new TokenModel()
        {
            Issued = DateTime.UtcNow,
            ExpiresIn = 60
        });

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new TokenModel()))
            });

        var sut = new RefreshTokenService(localStorage, new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://test")
        });

        // act
        var result = await sut.RefreshTokenAsync();

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task RefreshToken_Bad_Status_Code_Returns_False()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(new TokenModel());
        
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var sut = new RefreshTokenService(localStorage, new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://test")
        });
        
        // act
        var result = await sut.RefreshTokenAsync();

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task RefreshToken_Token_Expires_More_2_Minutes_Returns_False()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(new TokenModel()
        {
            Issued = DateTime.UtcNow,
            ExpiresIn = 180
        });

        var sut = new RefreshTokenService(localStorage, new HttpClient()
        {
            BaseAddress = new Uri("https://test")
        });

        // act
        var result = await sut.RefreshTokenAsync();

        // assert
        Assert.False(result);
    }
}