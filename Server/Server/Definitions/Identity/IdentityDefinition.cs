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
        var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
        if (dbSettings.UseInMemoryDb)
        {
            services.AddDbContext<UsersDbContext>(builder =>
            {
                builder.UseInMemoryDatabase(nameof(UsersDbContext));
            });
        }
        else
        {
            var connString = configuration.GetConnectionString("postgres");
            services.AddDbContext<UsersDbContext>(builder =>
            {
                builder.UseNpgsql(connString);
            });
        }
        
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