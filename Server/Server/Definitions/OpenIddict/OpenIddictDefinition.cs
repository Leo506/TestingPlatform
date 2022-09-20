using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;
using Server.Definitions.Database.Contexts;

namespace Server.Definitions.OpenIddict;

public class OpenIddictDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenIddict()
            .AddCore(builder =>
            {
                builder.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            })

            .AddServer(builder =>
            {
                builder.AllowPasswordFlow();
                builder.AllowRefreshTokenFlow();

                builder.SetTokenEndpointUris("/connect/token");

                builder.AcceptAnonymousClients();

                builder.AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey()
                    .DisableAccessTokenEncryption();

                builder.RegisterScopes("api");

                builder.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();
            })

            .AddValidation(builder =>
            {
                builder.UseLocalServer();
                builder.UseAspNetCore();
            });
    }
}