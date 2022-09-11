using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Server.Definitions.Database.Contexts;

namespace Server.Definitions.DataSeeding;

public class DataSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("admin", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor()
            {
                ClientId = "admin",
                ClientSecret = "admin-secret",
                DisplayName = "Admin",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                }
            }, cancellationToken);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}