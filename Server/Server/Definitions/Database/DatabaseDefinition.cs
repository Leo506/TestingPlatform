using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;
using Server.Definitions.Database.Contexts;
using Server.Models;

namespace Server.Definitions.Database;

public class DatabaseDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
        if (dbSettings.UseInMemoryDb)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseInMemoryDatabase(nameof(DbContext));
                builder.UseOpenIddict();
            });

            services.AddDbContext<UsersDbContext>(builder =>
            {
                builder.UseInMemoryDatabase(nameof(UsersDbContext));
            });
        }
        else
        {
            var connString = configuration.GetConnectionString("postgres");
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                
                builder.UseNpgsql(connString);
                builder.UseOpenIddict();
            });

            services.AddDbContext<UsersDbContext>(builder =>
            {
                builder.UseNpgsql(connString);
            });
        }
    }
}