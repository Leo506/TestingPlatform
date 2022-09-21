using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client;

public class TokenAuthProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationState(principal);
    }
}