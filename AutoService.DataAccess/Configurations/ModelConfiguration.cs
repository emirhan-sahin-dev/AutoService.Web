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
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(x => x.ModelId);

            builder.Property(x => x.ModelName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);


        }


    }
}
