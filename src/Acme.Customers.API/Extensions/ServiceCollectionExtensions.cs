using System.Reflection;
using Acme.Customers.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Acme.Customers.API.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CustomerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AcmeDatabase"), 
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                        sqlOptions.MigrationsAssembly("Acme.Customers.Infrastructure");
                    });
            }
        );

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        hcBuilder
            .AddSqlServer(configuration.GetConnectionString("AcmeDatabase"),
                name: "customers-db-check",
                tags: new string[] { "customers-db" },
                timeout: TimeSpan.FromSeconds(5));

        return services;
    }

    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "Provide API key to access",
                Type = SecuritySchemeType.ApiKey,
                Name = Constants.ApiKeyHeaderName,
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var req = new OpenApiSecurityRequirement
            {
                { scheme, new List<string>() }
            };
            c.AddSecurityRequirement(req);
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        return services;
    }
}