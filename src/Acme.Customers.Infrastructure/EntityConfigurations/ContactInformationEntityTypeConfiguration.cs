using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Customers.Infrastructure.EntityConfigurations;

public class ContactInformationEntityTypeConfiguration : IEntityTypeConfiguration<ContactInformation>
{
    public void Configure(EntityTypeBuilder<ContactInformation> entity)
    {
        entity.ToTable("contacts", CustomerContext.SCHEMA)
            .HasKey(x => x.CustomerId);
    }
}