using Client.Models;
using Client.Services;
using Client.Services.Interfaces;
using Client.UnitTests.Helpers;
using Moq;

namespace Client.UnitTests;

public class ResultServiceTests
{
    [Fact]
    public async Task SendResultAsync_No_TokenModel_Returns_False()
    {
        // arrange
        var sut = new ResultService(LocalStorageHelper.GetService(), HttpClientHelper.GetSimpleClient(),
            new Mock<IInterceptorService>().Object);

        // act
        var result = await sut.SendResultAsync(new ResultModel());

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task SendResultAsync_Bad_Response_Returns_False()
    {
        // arrange
        var sut = new ResultService(LocalStorageHelper.GetService(new TokenModel()
        {
            AccessToken = "1111"
        }), HttpClientHelper.GetBadClient(), new Mock<IInterceptorService>().Object);

        // act
        var result = await sut.SendResultAsync(new ResultModel());

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task SendResultAsync_All_Good_Returns_True()
    {
        // arrange
        var sut = new ResultService(LocalStorageHelper.GetService(new TokenModel()
        {
            AccessToken = "1111"
        }), HttpClientHelper.GetClient(""), new Mock<IInterceptorService>().Object);

        // act
        var result = await sut.SendResultAsync(new ResultModel());

        // assert
        Assert.True(result);
    }
}