namespace Server.Definitions.Base;

internal interface IAppDefinition
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    
    void ConfigureApplication(WebApplication app, IWebHostEnvironment env);
}