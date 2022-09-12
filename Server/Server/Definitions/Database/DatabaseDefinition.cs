using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;
using Server.Definitions.Database.Contexts;
using Server.Models;

namespace Server.Definitions.Database;

public class DatabaseDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(builder =>
        {
#if DEBUG
            builder.UseInMemoryDatabase(nameof(ApplicationDbContext));

#else
            var connString = configuration.GetConnectionString("postgres");
            builder.UseNpgsql(connString);
#endif
            builder.UseOpenIddict();
        });

        services.AddDbContext<UsersDbContext>(builder =>
        {
#if DEBUG
            builder.UseInMemoryDatabase(nameof(UsersDbContext));

#else
            var connString = configuration.GetConnectionString("postgres");
            builder.UseNpgsql(connString);
#endif
        });
    }
}