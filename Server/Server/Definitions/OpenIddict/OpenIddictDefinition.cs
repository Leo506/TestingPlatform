using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;

namespace Server.Definitions.OpenIddict;

public class OpenIddictDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenIddict()
            .AddCore(builder =>
            {
                builder.UseEntityFrameworkCore()
                    .UseDbContext<DbContext>();
            })

            .AddServer(builder =>
            {
                builder.AllowClientCredentialsFlow();

                builder.SetTokenEndpointUris("/connect/token");

                builder.AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey();

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