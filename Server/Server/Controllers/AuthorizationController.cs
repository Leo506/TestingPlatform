using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Server.Controllers;

public class AuthorizationController : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(IOpenIddictApplicationManager applicationManager,
        ILogger<AuthorizationController> logger)
    {
        _applicationManager = applicationManager;
        _logger = logger;
    }

    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Auth()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request is null)
        {
            _logger.LogError("Authorization request is null");
            return BadRequest();
        }
        
        if (!request.IsClientCredentialsGrantType())
            throw new NotImplementedException("The specified grant is not implemented");

        var client = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                     throw new InvalidOperationException("The client cannot be found");

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

        identity.AddClaim(OpenIddictConstants.Claims.Subject, await _applicationManager.GetClientIdAsync(client),
            OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);

        identity.AddClaim(OpenIddictConstants.Claims.Name, await _applicationManager.GetDisplayNameAsync(client),
            OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);

        return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}