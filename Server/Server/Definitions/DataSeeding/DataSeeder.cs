using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Server.Definitions.Database.Contexts;
using Server.Models;

namespace Server.Definitions.DataSeeding;

public class DataSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        
        var adminEmail = "admin@admin.com";
        var password = "12345678";
        var adminUserName = "admin";
        var userName = "user";
        var userEmail = "user@user.com";

        if (await roleManager.FindByNameAsync("Admin") == null)
            await roleManager.CreateAsync(new ApplicationRole("Admin"));
        
        if (await roleManager.FindByNameAsync("User") == null)
            await roleManager.CreateAsync(new ApplicationRole("User"));

        if (await userManager.FindByNameAsync(adminUserName) == null)
        {
            var user = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminUserName
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "Admin");
        }
        
        if (await userManager.FindByNameAsync(userName) == null)
        {
            var user = new ApplicationUser()
            {
                Email = userEmail,
                UserName = userName
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, "User");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}