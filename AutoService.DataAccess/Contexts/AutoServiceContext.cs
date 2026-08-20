using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Contexts
{
    public class AutoServiceContext : DbContext
    {
        public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
            : base(options)
        {

        }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Mechanic> Mechanics => Set<Mechanic>();
        public DbSet<ServiceRecord> ServiceRecords => Set<ServiceRecord>();
        public DbSet<ServiceDetail> ServiceDetails => Set<ServiceDetail>();
        public DbSet<SparePart> SpareParts => Set<SparePart>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<MechanicSpecialty> MechanicSpecialties
            => Set<MechanicSpecialty>();

        public DbSet<ServiceOperationType> ServiceOperationTypes
            => Set<ServiceOperationType>();

        public DbSet<ServiceOperation> ServiceOperations
            => Set<ServiceOperation>();

        public DbSet<Payment> Payments
            => Set<Payment>();

        public DbSet<ServiceOperationPart> ServiceOperationParts
            => Set<ServiceOperationPart>();

        public DbSet<Appointment> Appointments
            => Set<Appointment>();

        public DbSet<ServiceOperationTypeSparePart>
    ServiceOperationTypeSpareParts
        => Set<ServiceOperationTypeSparePart>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(x => x.SystemSettingId);

                entity.Property(x => x.CompanyName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.CompanyPhone)
                    .HasMaxLength(30);

                entity.Property(x => x.CompanyEmail)
                    .HasMaxLength(150);

                entity.Property(x => x.CompanyAddress)
                    .HasMaxLength(500);

                entity.Property(x => x.VatRate)
                    .HasColumnType("decimal(5,2)");

                entity.Property(x => x.Currency)
                    .IsRequired()
                    .HasMaxLength(10);
            });
            modelBuilder.Entity<ServiceOperationType>(entity =>
            {
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.DefaultDurationHours)
                    .HasColumnType("decimal(8,2)");

                entity.Property(x => x.CustomerLaborPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.MechanicPayment)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ServiceOperation>(entity =>
            {
                entity.Property(x => x.ProblemDescription)
                    .HasMaxLength(1000);

                entity.Property(x => x.WorkDescription)
                    .HasMaxLength(1000);

                entity.Property(x => x.LaborHours)
                    .HasColumnType("decimal(8,2)");

                entity.Property(x => x.CustomerLaborPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.MechanicPayment)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.LaborGrossMargin)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ServiceOperationPart>(entity =>
            {
                entity.Property(x => x.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.TotalPrice)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ServiceOperationTypeSparePart>(entity =>
            {
                entity.HasKey(x =>
                    x.ServiceOperationTypeSparePartId);

                entity
                    .HasOne(x => x.ServiceOperationType)
                    .WithMany(x => x.ServiceOperationTypeSpareParts)
                    .HasForeignKey(x => x.ServiceOperationTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(x => x.SparePart)
                    .WithMany(x => x.ServiceOperationTypeSpareParts)
                    .HasForeignKey(x => x.SparePartId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(x => x.AppointmentId);

                entity.Property(x => x.CustomerRequest)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);

                entity.Property(x => x.AppointmentDate)
                    .IsRequired();

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Appointments)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Vehicle)
                    .WithMany(x => x.Appointments)
                    .HasForeignKey(x => x.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ServiceRecord)
                    .WithOne(x => x.Appointment)
                    .HasForeignKey<Appointment>(x => x.ServiceRecordId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MechanicSpecialty>(entity =>
            {
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);
            });
            modelBuilder.Entity<ServiceRecord>(entity =>
            {
                entity.Property(x => x.VehicleDeliveredBy)
                    .HasMaxLength(150);

                entity.Property(x => x.VehicleDeliveredByPhone)
                    .HasMaxLength(30);

                entity.Property(x => x.ExistingDamages)
                    .HasMaxLength(1000);

                entity.Property(x => x.DeliveredItems)
                    .HasMaxLength(1000);

                entity.Property(x => x.AdvisorName)
                    .HasMaxLength(150);

                entity.Property(x => x.CustomerNotes)
                    .HasMaxLength(1000);

                entity.Property(x => x.PreApprovalLimit)
                    .HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => x.PaymentId);

                entity.Property(x => x.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.TransactionReference)
                    .HasMaxLength(100);

                entity.Property(x => x.PaymentMethod)
                    .IsRequired();

                entity.Property(x => x.PaymentDate)
                    .IsRequired();

                entity.HasOne(x => x.ServiceRecord)
                    .WithMany(x => x.Payments)
                    .HasForeignKey(x => x.ServiceRecordId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoServiceContext).Assembly);

            modelBuilder.Entity<Customer>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Vehicle>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Brand>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Model>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Mechanic>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceRecord>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceDetail>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<SparePart>()
                .HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MechanicSpecialty>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceOperationType>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceOperation>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Payment>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceOperationPart>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Appointment>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ServiceOperationTypeSparePart>()
                .HasQueryFilter(x => !x.IsDeleted);
        }
        public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    }


}
