using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using AutoService.Entity.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class AppointmentRepository
    : GenericRepository<Appointment>,
      IAppointmentRepository
{
    public AppointmentRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<List<Appointment>>
        GetAllWithDetailsAsync()
    {
        return await _context.Appointments
            .AsNoTracking()

            .Include(x => x.Customer)

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)

            .Where(x => !x.IsDeleted)

            .OrderBy(x => x.AppointmentDate)

            .ToListAsync();
    }

    public async Task<Appointment?>
        GetByIdWithDetailsAsync(
            int appointmentId)
    {
        return await _context.Appointments

            .Include(x => x.Customer)

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)

            .Include(x => x.ServiceRecord)

            .FirstOrDefaultAsync(x =>
                x.AppointmentId ==
                    appointmentId &&
                !x.IsDeleted);
    }

    public async Task<bool>
        HasVehicleTimeConflictAsync(
            int vehicleId,
            DateTime appointmentDate,
            int? excludedAppointmentId = null)
    {
        /*
         * Aynı araç için aynı saat çevresinde
         * başka aktif randevu var mı kontrol eder.
         *
         * 30 dakikalık tolerans kullanıyoruz.
         */
        var startTime =
            appointmentDate.AddMinutes(-30);

        var endTime =
            appointmentDate.AddMinutes(30);

        return await _context.Appointments
            .AsNoTracking()
            .AnyAsync(x =>
                !x.IsDeleted &&

                x.VehicleId ==
                    vehicleId &&

                x.AppointmentDate >=
                    startTime &&

                x.AppointmentDate <=
                    endTime &&

                x.Status !=
                    AppointmentStatus.Cancelled &&

                (!excludedAppointmentId.HasValue ||
                 x.AppointmentId !=
                    excludedAppointmentId.Value));
    }
}