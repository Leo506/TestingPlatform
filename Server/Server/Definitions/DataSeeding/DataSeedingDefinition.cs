using Server.Definitions.Base;

namespace Server.Definitions.DataSeeding;

public class DataSeedingDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services.AddHostedService<DataSeeder>();
}