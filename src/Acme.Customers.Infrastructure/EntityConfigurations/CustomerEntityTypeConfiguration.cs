using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Customers.Infrastructure.EntityConfigurations;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.ToTable("customers", CustomerContext.SCHEMA)
            .HasKey(x => x.Id);

        entity.HasOne(x => x.ContactInformation)
            .WithOne()
            .HasForeignKey<ContactInformation>(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasMany(x => x.Orders)
            .WithOne()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}