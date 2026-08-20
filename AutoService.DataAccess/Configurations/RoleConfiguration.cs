using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.RoleId);

        builder.Property(x => x.RoleName)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.RoleName)
            .IsUnique();
        builder.HasData(
    new Role
    {
        RoleId = 1,
        RoleName = "Admin",
        CreatedDate = new DateTime(2026, 1, 1),
        IsDeleted = false
    },
    new Role
    {
        RoleId = 2,
        RoleName = "Service Advisor",
        CreatedDate = new DateTime(2026, 1, 1),
        IsDeleted = false
    },
    new Role
    {
        RoleId = 3,
        RoleName = "Mechanic",
        CreatedDate = new DateTime(2026, 1, 1),
        IsDeleted = false
    });
    }
}
