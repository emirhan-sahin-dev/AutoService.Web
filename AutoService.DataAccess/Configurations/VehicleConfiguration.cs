using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(x => x.VehicleId);

            builder.Property(x => x.Plate)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasIndex(x => x.Plate)
                .IsUnique();

            builder.Property(x => x.VinNumber)
                .IsRequired()
                .HasMaxLength(17);

            builder.HasIndex(x => x.VinNumber)
                .IsUnique();

            builder.Property(x => x.ModelYear)
                .IsRequired();

            builder.Property(x => x.Mileage)
                .IsRequired();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Model)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
