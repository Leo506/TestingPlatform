using Client.LocalStorage;
using Client.Models;
using Moq;

namespace Client.UnitTests.Helpers;

public static class LocalStorageHelper
{
    public static ILocalStorageService GetService()
    {
        return new Mock<ILocalStorageService>().Object;
    }

    public static ILocalStorageService GetService(TokenModel returnValue)
    {
        var mock = new Mock<ILocalStorageService>();
        mock.Setup(x => x.GetAsync<TokenModel>(It.IsAny<string>()))
            .Returns(Task.FromResult<TokenModel?>(returnValue));

        return mock.Object;
    }
}