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
    public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
    {
        public void Configure(EntityTypeBuilder<Mechanic> builder)
        {
            builder.HasKey(x => x.MechanicId);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .HasMaxLength(50);

            builder.Property(x => x.Specialty)
                .HasMaxLength(100);
        }
    }
}
