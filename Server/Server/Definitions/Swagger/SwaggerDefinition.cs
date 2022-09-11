using Server.Definitions.Base;

namespace Server.Definitions.Swagger;

public class SwaggerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public override void ConfigureApplication(WebApplication app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}