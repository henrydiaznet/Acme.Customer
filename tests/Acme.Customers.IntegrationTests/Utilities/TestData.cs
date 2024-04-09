using Acme.Customers.API.Extensions;
using Acme.Customers.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Customers.IntegrationTests.Utilities;

public static class TestData
{
    public static void Init(IServiceScope scope)
    {
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CustomerContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        WebApplicationExtensions.SeedData(db);
    }
    
    public static void Init(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CustomerContext>();
        WebApplicationExtensions.SeedData(db);
    }
}