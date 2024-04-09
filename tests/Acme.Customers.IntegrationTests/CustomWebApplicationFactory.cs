using System.Data.Common;
using Acme.Customers.API.Authentication.Contracts;
using Acme.Customers.Infrastructure;
using Acme.Customers.IntegrationTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acme.Customers.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "key", "value" }
            })
            .Build();

        builder.UseConfiguration(config);
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IApiKeyValidator>();
            services.AddSingleton<IApiKeyValidator,MockApiKeyValidator>();
            
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CustomerContext>));
            services.Remove(dbContextDescriptor);
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);
            
            services.AddDbContext<CustomerContext>(options =>
            {
                options.UseInMemoryDatabase("acme");
            });
        });

        builder.UseEnvironment("test");
    }
}
