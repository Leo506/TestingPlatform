using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;
using Server.Definitions.Database;
using Server.Definitions.Database.Contexts;
using Server.Models;

namespace Server.Definitions.Identity;

public class IdentityDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<UsersDbContext>();
    }

    public override void ConfigureApplication(WebApplication app, IWebHostEnvironment env)
    {
    }
}