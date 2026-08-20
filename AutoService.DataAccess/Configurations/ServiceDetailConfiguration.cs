using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations;

public class ServiceDetailConfiguration : IEntityTypeConfiguration<ServiceDetail>
{
    public void Configure(EntityTypeBuilder<ServiceDetail> builder)
    {
        builder.HasKey(x => x.ServiceDetailId);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.ServiceRecord)
             .WithMany()
             .HasForeignKey(x => x.ServiceRecordId)
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SparePart)
            .WithMany(x => x.ServiceDetails)
            .HasForeignKey(x => x.SparePartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}