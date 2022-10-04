using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Server.Data;
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
        
        const string adminEmail = "admin@admin.com";
        const string adminUserName = "admin";
        const string studentUserName = "student";
        const string studentEmail = "user@user.com";
        const string teacherUserName = "teacher";
        const string teacherEmail = "teacher@teacher.com";
        const string password = "12345678";

        if (await roleManager.FindByNameAsync(ServerConstants.Roles.Admin) == null)
            await roleManager.CreateAsync(new ApplicationRole(ServerConstants.Roles.Admin));
        
        if (await roleManager.FindByNameAsync(ServerConstants.Roles.Student) == null)
            await roleManager.CreateAsync(new ApplicationRole(ServerConstants.Roles.Student));

        if (await roleManager.FindByNameAsync(ServerConstants.Roles.Teacher) == null)
            await roleManager.CreateAsync(new ApplicationRole(ServerConstants.Roles.Teacher));

        if (await userManager.FindByNameAsync(adminUserName) == null)
        {
            var user = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminUserName
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, ServerConstants.Roles.Admin);
        }
        
        if (await userManager.FindByNameAsync(studentUserName) == null)
        {
            var user = new ApplicationUser()
            {
                Email = studentEmail,
                UserName = studentUserName
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, ServerConstants.Roles.Student);
        }
        
        if (await userManager.FindByNameAsync(teacherUserName) == null)
        {
            var user = new ApplicationUser()
            {
                Email = teacherEmail,
                UserName = teacherUserName
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, ServerConstants.Roles.Teacher);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}