using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Customers.Infrastructure.EntityConfigurations;

public class StockItemEntityTypeConfiguration : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> entity)
    {
        entity.ToTable("stock_items", CustomerContext.SCHEMA)
            .HasKey(x => x.Id);
    }
}