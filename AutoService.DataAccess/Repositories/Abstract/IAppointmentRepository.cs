using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IAppointmentRepository
    : IGenericRepository<Appointment>
{
    Task<List<Appointment>>
        GetAllWithDetailsAsync();

    Task<Appointment?>
        GetByIdWithDetailsAsync(
            int appointmentId);

    Task<bool>
        HasVehicleTimeConflictAsync(
            int vehicleId,
            DateTime appointmentDate,
            int? excludedAppointmentId = null);
}
