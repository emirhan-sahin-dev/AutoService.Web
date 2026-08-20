using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations;

public class SparePartConfiguration : IEntityTypeConfiguration<SparePart>
{
    public void Configure(EntityTypeBuilder<SparePart> builder)
    {
        builder.HasKey(x => x.SparePartId);

        builder.Property(x => x.PartName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PartCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.PartCode)
            .IsUnique();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");
    }
}