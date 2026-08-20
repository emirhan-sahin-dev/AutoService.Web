using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasData(
            new User
            {
                UserId = 1,
                FullName = "Sistem Yöneticisi",
                Username = "admin",
                Email = "admin@autoservice.com",
                PasswordHash = "$2a$11$DUwjgQnB9o3B/jFU3bqPd.fzhQz6yiUY.5jLPS9xlmItfDZIup6Jm",
                IsActive = true,
                RoleId=1,
                CreatedDate=new DateTime(2026, 1, 1),
                IsDeleted=false
            });
    }
}