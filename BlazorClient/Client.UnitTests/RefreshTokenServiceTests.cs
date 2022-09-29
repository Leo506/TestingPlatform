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

        var client = HttpClientHelper.GetClient(new TokenModel());

        var sut = new RefreshTokenService(localStorage, client);

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

        var client = HttpClientHelper.GetBadClient();

        var sut = new RefreshTokenService(localStorage, client);
        
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

        var sut = new RefreshTokenService(localStorage, HttpClientHelper.GetSimpleClient());

        // act
        var result = await sut.RefreshTokenAsync();

        // assert
        Assert.False(result);
    }
}