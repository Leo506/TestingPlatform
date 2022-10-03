using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using OpenIddict.Abstractions;

namespace Server.UnitTests.Helpers;

public static class HttpContextHelper
{
    public static HttpContext GetContext()
    {
        var mock = new Mock<HttpContext>();
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, "9999"));
        var principal = new ClaimsPrincipal(identity);
        mock.Setup(context => context.User).Returns(principal);

        return mock.Object;
    }
}
