using Acme.Customers.Domain.Model;
using Acme.Customers.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Acme.Customers.API.Extensions;

public static class WebApplicationExtensions
{
    public static void InitializeDatabase(this WebApplication app)
    {
        if (app.Environment.EnvironmentName == "test")
        {
            return;
        }
        
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<CustomerContext>();
        context.Database.Migrate();
        SeedData(context);
    }

    public static void SeedData(CustomerContext context)
    {
        var items = new List<StockItem>
        {
            new() { Description = "Wool socks" },
            new() { Description = "Wool boxers" },
            new() { Description = "Wool suit" },
            new() { Description = "Fake beard" },
            new() { Description = "Lawn gnome" },
            new() { Description = "Chainsaw" },
            new() { Description = "Golf balls" },
            new() { Description = "Garden table" },
            new() { Description = "Santa Suit" }
        };

        context.StockItems.AddRange(items);

        var customers = new List<Customer>
        {
            new("John Boe", new ContactInformation
            {
                Address = "Boe St., 15, 90300",
                Email = "john_boe@email.com",
                Phone = "(145)123-1234"
            }, new List<Order>
            {
                new(new List<OrderItem>
                {
                    new() { Quantity = 1, ItemId = 1 },
                    new() { Quantity = 2, ItemId = 2 },
                    new() { Quantity = 1, ItemId = 3 }
                }),
                new(new List<OrderItem>
                {
                    new() { Quantity = 1, ItemId = 9 },
                    new() { Quantity = 1, ItemId = 6 }
                })
            }),
            new("Jane Moe", new ContactInformation
            {
                Address = "Moe St., 16, 91115",
                Email = "jane_moe@email.com",
                Phone = "(52)123-1234"
            }, new List<Order>
            {
                new(new List<OrderItem>
                {
                    new() { Quantity = 1, ItemId = 4 },
                    new() { Quantity = 2, ItemId = 5 },
                    new() { Quantity = 1, ItemId = 7 }
                }),
                new(new List<OrderItem>
                {
                    new() { Quantity = 1, ItemId = 8 },
                    new() { Quantity = 1, ItemId = 4 }
                })
            }),
            new("Bob Poe", new ContactInformation
            {
                Address = "Poe st., 52, 7874",
                Email = "bob_poe@email.com",
                Phone = "123-23422"
            }, Array.Empty<Order>()),
            new("Pope Joe", new ContactInformation
            {
                Address = "Pope st., 52, 4255",
                Email = "pope@email.com",
                Phone = "123-2342221"
            }, Array.Empty<Order>()),
            new("Alice Bob", new ContactInformation
            {
                Address = "Bob st., 52, 12312",
                Email = "Alice_bob@email.com",
                Phone = "3221-1212"
            }, new List<Order>
            {
                new(new List<OrderItem>
                {
                    new() { Quantity = 1, ItemId = 4 },
                    new() { Quantity = 2, ItemId = 5 },
                    new() { Quantity = 1, ItemId = 7 }
                })
            }),
            new("Delete Me", new ContactInformation
            {
                Address = "Delete st., 212, 22",
                Email = "Delete@email.com",
                Phone = "123-123"
            }, Array.Empty<Order>()),
        };

        foreach (var order in customers[0].Orders) order.FulfillOrder();

        customers[1].Orders.First().FulfillOrder();

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}