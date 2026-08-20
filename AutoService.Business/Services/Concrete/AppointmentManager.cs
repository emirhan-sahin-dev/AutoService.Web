using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.AppointmentDtos;
using AutoService.Entity.Entities;
using AutoService.Entity.Enums;

namespace AutoService.Business.Services.Concrete;

public class AppointmentManager
    : IAppointmentService
{
    private readonly IAppointmentRepository
        _appointmentRepository;

    private readonly ICustomerRepository
        _customerRepository;

    private readonly IVehicleRepository
        _vehicleRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    private readonly IServiceRecordRepository
    _serviceRecordRepository;

    public AppointmentManager(
        IAppointmentRepository appointmentRepository,
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IServiceRecordRepository serviceRecordRepository)
    {
        _appointmentRepository =
            appointmentRepository;

        _customerRepository =
            customerRepository;

        _vehicleRepository =
            vehicleRepository;

        _unitOfWork =
            unitOfWork;

        _serviceRecordRepository =
           serviceRecordRepository;
    }

    public async Task<List<ResultAppointmentDto>>
        GetAllAsync()
    {
        var values =
            await _appointmentRepository
                .GetAllWithDetailsAsync();

        return values
            .Select(x => new ResultAppointmentDto
            {
                AppointmentId =
                    x.AppointmentId,

                CustomerId =
                    x.CustomerId,

                CustomerName =
                    x.Customer?.FullName ?? "-",

                VehicleId =
                    x.VehicleId,

                Plate =
                    x.Vehicle?.Plate ?? "-",

                BrandName =
                    x.Vehicle?
                        .Model?
                        .Brand?
                        .BrandName ?? "-",

                ModelName =
                    x.Vehicle?
                        .Model?
                        .ModelName ?? "-",

                AppointmentDate =
                    x.AppointmentDate,

                CustomerRequest =
                    x.CustomerRequest,

                Status =
                    x.Status,

                ServiceRecordId =
                    x.ServiceRecordId
            })
            .ToList();
    }

    public async Task<AppointmentDetailDto?>
        GetByIdAsync(
            int appointmentId)
    {
        var value =
            await _appointmentRepository
                .GetByIdWithDetailsAsync(
                    appointmentId);

        if (value == null ||
            value.IsDeleted)
        {
            return null;
        }

        return new AppointmentDetailDto
        {
            AppointmentId =
                value.AppointmentId,

            CustomerId =
                value.CustomerId,

            CustomerName =
                value.Customer?.FullName ?? "-",

            CustomerPhone =
                value.Customer?.Phone ?? "-",

            CustomerEmail =
                value.Customer?.Email ?? "-",

            VehicleId =
                value.VehicleId,

            Plate =
                value.Vehicle?.Plate ?? "-",

            VinNumber =
                value.Vehicle?.VinNumber ?? "-",

            BrandName =
                value.Vehicle?
                    .Model?
                    .Brand?
                    .BrandName ?? "-",

            ModelName =
                value.Vehicle?
                    .Model?
                    .ModelName ?? "-",

            ModelYear =
                value.Vehicle?.ModelYear ?? 0,

            AppointmentDate =
                value.AppointmentDate,

            CustomerRequest =
                value.CustomerRequest,

            Description =
                value.Description,

            Status =
                value.Status,

            ServiceRecordId =
                value.ServiceRecordId,

            CreatedDate =
                value.CreatedDate
        };
    }

    public async Task AddAsync(
        CreateAppointmentDto dto)
    {
        var customer =
            await _customerRepository
                .GetByIdAsync(
                    dto.CustomerId);

        if (customer == null ||
            customer.IsDeleted)
        {
            throw new Exception(
                "Müşteri bulunamadı.");
        }

        var vehicle =
            await _vehicleRepository
                .GetByIdAsync(
                    dto.VehicleId);

        if (vehicle == null ||
            vehicle.IsDeleted)
        {
            throw new Exception(
                "Araç bulunamadı.");
        }

        if (vehicle.CustomerId !=
            dto.CustomerId)
        {
            throw new Exception(
                "Seçilen araç bu müşteriye ait değil.");
        }

        if (dto.AppointmentDate <=
            DateTime.Now)
        {
            throw new Exception(
                "Randevu tarihi geçmiş bir tarih olamaz.");
        }

        var hasConflict =
            await _appointmentRepository
                .HasVehicleTimeConflictAsync(
                    dto.VehicleId,
                    dto.AppointmentDate);

        if (hasConflict)
        {
            throw new Exception(
                "Bu araç için seçilen saate yakın başka bir aktif randevu bulunuyor.");
        }

        var entity =
            new Appointment
            {
                CustomerId =
                    dto.CustomerId,

                VehicleId =
                    dto.VehicleId,

                AppointmentDate =
                    dto.AppointmentDate,

                CustomerRequest =
                    dto.CustomerRequest.Trim(),

                Description =
                    dto.Description?.Trim(),

                Status =
                    dto.Status,

                ServiceRecordId =
                    null,

                CreatedDate =
                    DateTime.Now,

                IsDeleted =
                    false
            };

        await _appointmentRepository
            .AddAsync(entity);

        await _unitOfWork
            .SaveChangesAsync();
    }

    public async Task UpdateAsync(
        UpdateAppointmentDto dto)
    {
        var entity =
            await _appointmentRepository
                .GetByIdAsync(
                    dto.AppointmentId);

        if (entity == null ||
            entity.IsDeleted)
        {
            throw new Exception(
                "Randevu bulunamadı.");
        }

        if (entity.ServiceRecordId.HasValue)
        {
            throw new Exception(
                "Servis kaydına dönüştürülmüş randevu güncellenemez.");
        }

        var customer =
            await _customerRepository
                .GetByIdAsync(
                    dto.CustomerId);

        if (customer == null ||
            customer.IsDeleted)
        {
            throw new Exception(
                "Müşteri bulunamadı.");
        }

        var vehicle =
            await _vehicleRepository
                .GetByIdAsync(
                    dto.VehicleId);

        if (vehicle == null ||
            vehicle.IsDeleted)
        {
            throw new Exception(
                "Araç bulunamadı.");
        }

        if (vehicle.CustomerId !=
            dto.CustomerId)
        {
            throw new Exception(
                "Seçilen araç bu müşteriye ait değil.");
        }

        if (dto.AppointmentDate <=
                DateTime.Now &&
            dto.Status !=
                Entity.Enums.AppointmentStatus.Cancelled)
        {
            throw new Exception(
                "Aktif randevunun tarihi geçmişte olamaz.");
        }

        var hasConflict =
            await _appointmentRepository
                .HasVehicleTimeConflictAsync(
                    dto.VehicleId,
                    dto.AppointmentDate,
                    dto.AppointmentId);

        if (hasConflict)
        {
            throw new Exception(
                "Bu araç için seçilen saate yakın başka bir aktif randevu bulunuyor.");
        }

        entity.CustomerId =
            dto.CustomerId;

        entity.VehicleId =
            dto.VehicleId;

        entity.AppointmentDate =
            dto.AppointmentDate;

        entity.CustomerRequest =
            dto.CustomerRequest.Trim();

        entity.Description =
            dto.Description?.Trim();

        entity.Status =
            dto.Status;

        entity.UpdatedDate =
            DateTime.Now;

        _appointmentRepository
            .Update(entity);

        await _unitOfWork
            .SaveChangesAsync();
    }

    public async Task DeleteAsync(
        int appointmentId)
    {
        var entity =
            await _appointmentRepository
                .GetByIdAsync(
                    appointmentId);

        if (entity == null ||
            entity.IsDeleted)
        {
            throw new Exception(
                "Randevu bulunamadı.");
        }

        if (entity.ServiceRecordId.HasValue)
        {
            throw new Exception(
                "Servis kaydına dönüştürülmüş randevu silinemez.");
        }

        await _appointmentRepository
            .SoftDeleteAsync(entity);

        await _unitOfWork
            .SaveChangesAsync();
    }

    public async Task<int> ConvertToServiceRecordAsync(
    int appointmentId)
    {
        var appointment =
            await _appointmentRepository
                .GetByIdWithDetailsAsync(
                    appointmentId);

        if (appointment == null ||
            appointment.IsDeleted)
        {
            throw new Exception(
                "Randevu bulunamadı.");
        }

        if (appointment.ServiceRecordId.HasValue)
        {
            throw new Exception(
                "Bu randevu daha önce servis kaydına dönüştürülmüş.");
        }

        if (appointment.Status ==
            AppointmentStatus.Cancelled)
        {
            throw new Exception(
                "İptal edilmiş randevu servis kaydına dönüştürülemez.");
        }

        var serviceRecord =
            new ServiceRecord
            {
                VehicleId =
                    appointment.VehicleId,

                CheckInDate =
                    DateTime.Now,

                EstimatedDeliveryDate =
                    DateTime.Now.AddDays(1),

                ActualDeliveryDate =
                    null,

                Mileage =
                    appointment.Vehicle?.Mileage ?? 0,

                CustomerComplaint =
                    appointment.CustomerRequest,

                Description =
                    appointment.Description,

                Status =
                    ServiceStatus.Bekliyor,

                FuelLevel =
                    FuelLevel.Bos,

                ExistingDamages =
                    null,

                DeliveredItems =
                    null,

                AdvisorName =
                    null,

                CustomerNotes =
                    null,

                VehicleDeliveredBy =
                    appointment.Customer?.FullName,

                VehicleDeliveredByPhone =
                    appointment.Customer?.Phone,

                PreApprovalLimit =
                    0,

                RequiresApprovalForExtraWork =
                    true,

                ReturnOldPartsToCustomer =
                    false,

                LaborCost =
                    0,

                TotalPrice =
                    0,

                CreatedDate =
                    DateTime.Now,

                IsDeleted =
                    false
            };

        await _serviceRecordRepository
            .AddAsync(serviceRecord);

        await _unitOfWork
            .SaveChangesAsync();

        appointment.ServiceRecordId =
            serviceRecord.ServiceRecordId;

        appointment.Status =
            AppointmentStatus.Arrived;

        appointment.UpdatedDate =
            DateTime.Now;

        _appointmentRepository
            .Update(appointment);

        await _unitOfWork
            .SaveChangesAsync();

        return serviceRecord.ServiceRecordId;
    }

}