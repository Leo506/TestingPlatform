using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Server.Definitions.Base;
using Server.Definitions.Database.Contexts;
using Server.Definitions.Database.Repositories;
using Server.Definitions.Database.Settings;
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
            Console.WriteLine(connString);
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
            Console.WriteLine(connString);
            builder.UseNpgsql(connString);
#endif
        });


        services.AddDbContext<ResultsDbContext>(builder =>
        {
#if DEBUG
            builder.UseInMemoryDatabase(nameof(ResultsDbContext));
#else
            var connString = configuration.GetConnectionString("postgres");
            Console.WriteLine(connString);
            builder.UseNpgsql(connString);
#endif
        });

        services.AddTransient<IMongoClient>(builder =>
            {
                var connString = configuration.GetConnectionString("mongo");
                return new MongoClient(connString);
            })
            .AddSingleton<IRepository<TestsModel>, TestsRepository>(provider =>
            {
                var mongoSettings = configuration.GetSection("MongoDbSettings").Get<MongoSettings>();
                var mongoClient = provider.GetRequiredService<IMongoClient>();

                return new TestsRepository(mongoClient, mongoSettings);
            });
    }
}