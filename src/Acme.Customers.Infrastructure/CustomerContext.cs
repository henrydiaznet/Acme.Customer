using System.Reflection;
using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Acme.Customers.Infrastructure;

public class CustomerContext : DbContext
{
    public const string SCHEMA = "customers";

    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ContactInformation> ContactInformations { get; set; }
    public DbSet<StockItem> StockItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}