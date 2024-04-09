using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Customers.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.ToTable("orders", CustomerContext.SCHEMA)
            .HasKey(x => x.Id);

        entity.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .OnDelete(DeleteBehavior.Cascade);

        entity.Property(x => x.Status)
            .HasConversion<string>();
    }
}