using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Customers.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> entity)
    {
        entity.ToTable("order_items", CustomerContext.SCHEMA)
            .HasKey(x => new { x.OrderId, x.ItemId });

        entity.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId);

        entity.HasOne(x => x.StockItem)
            .WithMany()
            .HasForeignKey(x => x.ItemId);
    }
}