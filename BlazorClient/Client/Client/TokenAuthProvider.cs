using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Client.LocalStorage;
using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client;

public class TokenAuthProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public TokenAuthProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenModel = await _localStorage.GetAsync<TokenModel>(nameof(TokenModel));
        if (tokenModel is null)
            return CreateAnonymous();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(tokenModel.AccessToken);

        var identity = new ClaimsIdentity(jwt.Claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);

        return new AuthenticationState(principal);
    }

    private AuthenticationState CreateAnonymous()
    {
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }
}