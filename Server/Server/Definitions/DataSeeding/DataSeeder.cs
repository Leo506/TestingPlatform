using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Server.Definitions.Database.Contexts;
using Server.Models;

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

        var usersContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        
        if (usersContext.UserInfos.Any())
            return;

        var createdClient = await manager.FindByClientIdAsync("admin", cancellationToken);
        var clientId = await manager.GetIdAsync(createdClient!, cancellationToken);
        var admin = new UserInfo()
        {
            UserId = clientId!,
            RoleId = 1
        };

        var adminRole = new RoleInfo()
        {
            RoleId = 1,
            RoleName = "Admin"
        };
        
        adminRole.UserInfos.Add(admin);

        await usersContext.UserInfos.AddAsync(admin, cancellationToken);
        await usersContext.RoleInfos.AddAsync(adminRole, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}