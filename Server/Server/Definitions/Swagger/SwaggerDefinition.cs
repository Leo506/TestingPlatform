using Microsoft.OpenApi.Models;
using Server.Definitions.Base;

namespace Server.Definitions.Swagger;

public class SwaggerDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        
        // TODO learn more about swagger and OAuth2.0
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Demo API",
                Version = "v1"
            });

            var authUrl = configuration.GetSection("AuthServer").Value;
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{authUrl}/connect/token", UriKind.Absolute)
                    }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }

    public override void ConfigureApplication(WebApplication app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}