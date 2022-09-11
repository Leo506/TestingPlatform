using Microsoft.EntityFrameworkCore;
using Server.Definitions.Base;

namespace Server.Definitions.Database;

public class DatabaseDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection("Database").Get<DatabaseSettings>();
        if (dbSettings.UseInMemoryDb)
        {
            services.AddDbContext<DbContext>(builder =>
            {
                builder.UseInMemoryDatabase(nameof(DbContext));
                builder.UseOpenIddict();
            });
            
            // Here will be add another db contexts
        }
        else
        {
            services.AddDbContext<DbContext>(builder =>
            {
                var connString = configuration.GetConnectionString("postgres");
                builder.UseNpgsql(connString);

                builder.UseOpenIddict();
            });
            
            // Here will be add another db contexts
        }
    }
}