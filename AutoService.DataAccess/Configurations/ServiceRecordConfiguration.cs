using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations;

public class ServiceRecordConfiguration
    : IEntityTypeConfiguration<ServiceRecord>
{
    public void Configure(
        EntityTypeBuilder<ServiceRecord> builder)
    {
        builder.HasKey(x => x.ServiceRecordId);

        builder.Property(x => x.CheckInDate)
            .IsRequired();

        builder.Property(x => x.EstimatedDeliveryDate)
            .IsRequired();

        builder.Property(x => x.ActualDeliveryDate)
            .IsRequired(false);

        builder.Property(x => x.Mileage)
            .IsRequired();

        builder.Property(x => x.CustomerComplaint)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.LaborCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Vehicle)
            .WithMany(x => x.ServiceRecords)
            .HasForeignKey(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ServiceOperations)
            .WithOne(x => x.ServiceRecord)
            .HasForeignKey(x => x.ServiceRecordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}