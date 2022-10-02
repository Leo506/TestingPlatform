using System.Net;
using Client.LocalStorage;
using Client.Models;
using Client.Services;
using Client.Services.Interfaces;
using Client.UnitTests.Helpers;
using Client.ViewModels;
using Moq;
using Moq.Protected;

namespace Client.UnitTests;

public class TestsServiceTests
{
    #region GetAllTests
    [Fact]
    public async Task GetAllTests_No_TokenModel_Returns_Empty()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(null);
        var interceptorService = new Mock<IInterceptorService>();

        var sut = new TestsService(new HttpClient()
        {
            BaseAddress = new Uri("https://test")
        }, localStorage, interceptorService.Object);

        // act
        var result = await sut.GetAllTests();
        var actual = result!.Any();

        // assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GetAllTests_Bad_Response_Returns_Empty()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(new TokenModel());
        var interceptorService = new Mock<IInterceptorService>();

        var client = HttpClientHelper.GetBadClient();

        var sut = new TestsService(client, localStorage, interceptorService.Object);

        // act
        var result = await sut.GetAllTests();
        var actual = result!.Any();

        // assert
        Assert.False(actual);
    }

    [Fact]
    public async Task GetAllTests_All_Good_Returns_Not_Empty()
    {
        // arrange
        var localStorage = LocalStorageHelper.GetService(new TokenModel()
        {
            AccessToken = "1111"
        });
        var interceptorService = new Mock<IInterceptorService>();

        var client = HttpClientHelper.GetClient<IEnumerable<TestViewModel>>(new[]
        {
            new TestViewModel()
        });

        var sut = new TestsService(client, localStorage, interceptorService.Object);

        // act
        var result = await sut.GetAllTests();
        var actual = result!.Any();

        // assert
        Assert.True(actual);
    }
    #endregion

    #region CreateTest

    [Fact]
    public async Task CreateTest_No_TokenModel_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetSimpleClient(), LocalStorageHelper.GetService(),
            interceptorService.Object);

        // act
        var result = await sut.CreateTest(new TestsModel());

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task CreateTest_Bad_Response_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetBadClient(), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.CreateTest(new TestsModel());

        // assert
        Assert.False(result);
    }


    [Fact]
    public async Task CreateTest_All_Good_Return_True()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetClient(""), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.CreateTest(new TestsModel());

        // assert
        Assert.True(result);
    }

    #endregion

    #region UpdateTest

    [Fact]
    public async Task UpdateTest_No_TokenModel_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetSimpleClient(), LocalStorageHelper.GetService(),
            interceptorService.Object);

        // act
        var result = await sut.UpdateTest(new TestsModel());

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateTest_Bad_Response_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetBadClient(), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.UpdateTest(new TestsModel());

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateTest_All_Good_Returns_True()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetClient(""), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.UpdateTest(new TestsModel());

        // assert
        Assert.True(result);
    }

    #endregion

    #region DeleteTest

    [Fact]
    public async Task DeleteTest_No_TokenModel_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetSimpleClient(), LocalStorageHelper.GetService(),
            interceptorService.Object);

        // act
        var result = await sut.DeleteTest("");

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteTest_Bad_Response_Returns_False()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetBadClient(), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.DeleteTest("");

        // assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteTest_All_Good_Returns_True()
    {
        // arrange
        var interceptorService = new Mock<IInterceptorService>();
        var sut = new TestsService(HttpClientHelper.GetClient(""), LocalStorageHelper.GetService(new TokenModel()
            {
                AccessToken = "1111"
            }),
            interceptorService.Object);

        // act
        var result = await sut.DeleteTest("");

        // assert
        Assert.True(result);
    }

    #endregion
}