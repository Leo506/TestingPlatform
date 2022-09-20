using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Server.Data;
using Server.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Server.Controllers;

public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ILogger<AuthorizationController> _logger;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthorizationController(ILogger<AuthorizationController> logger,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
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
        
        if (!request.IsPasswordGrantType() && !request.IsRefreshTokenGrantType())
            throw new NotImplementedException("The specified grant is not implemented");

        if (request.IsRefreshTokenGrantType())
        {
            var claimsPrincipal =
                (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                .Principal;

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>()
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    ServerConstants.ExceptionTexts.AuthFailed
            });

            return Forbid(properties);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            var properties = new AuthenticationProperties(new Dictionary<string, string?>()
            {
                [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                    ServerConstants.ExceptionTexts.AuthFailed
            });
            
            return Forbid(properties);
        }

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name, Claims.Role);

        identity.AddClaim(Claims.Subject, await _userManager.GetUserIdAsync(user), Destinations.AccessToken,
                Destinations.IdentityToken)
            .AddClaim(Claims.Email, await _userManager.GetEmailAsync(user), Destinations.AccessToken,
                Destinations.IdentityToken)
            .AddClaim(Claims.Username, await _userManager.GetUserNameAsync(user), Destinations.AccessToken,
                Destinations.IdentityToken);
        
        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            identity.AddClaim(Claims.Role, role, Destinations.AccessToken, Destinations.IdentityToken);
        }

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(Scopes.OfflineAccess);
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}