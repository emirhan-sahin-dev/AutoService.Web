using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.CustomerId);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .HasMaxLength(250);
    }
}