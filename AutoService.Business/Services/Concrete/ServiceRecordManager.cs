using AutoService.Dto.PaymentDtos;
using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.DataAccess.Repositories.Interfaces;
using AutoService.Dto.ServiceRecordDtos;
using AutoService.Entity.Entities;
using AutoService.Entity.Enums;

namespace AutoService.Business.Services.Concrete;

public class ServiceRecordManager : IServiceRecordService
{
    private readonly IServiceRecordRepository _serviceRecordRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemSettingRepository _systemSettingRepository;
    private readonly IPaymentRepository _paymentRepository;

    public ServiceRecordManager(
        IServiceRecordRepository serviceRecordRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ISystemSettingRepository systemSettingRepository,
        IPaymentRepository paymentRepository)
    {
        _serviceRecordRepository = serviceRecordRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _systemSettingRepository = systemSettingRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<List<ResultServiceRecordDto>> GetAllAsync()
    {
        var values =
            await _serviceRecordRepository.GetAllWithDetailsAsync();

        return values.Select(serviceRecord =>
        {
            var activeOperations = serviceRecord.ServiceOperations
                .Where(x => !x.IsDeleted)
                .ToList();

            var totalLaborPrice = activeOperations
                .Sum(x => x.CustomerLaborPrice);

            var totalPartsPrice = activeOperations
                .SelectMany(x => x.ServiceOperationParts)
                .Where(x => !x.IsDeleted)
                .Sum(x => x.TotalPrice);

            var totalMechanicPayment = activeOperations
                .Sum(x => x.MechanicPayment);

            var totalLaborGrossMargin = activeOperations
                .Sum(x => x.LaborGrossMargin);

            return new ResultServiceRecordDto
            {
                ServiceRecordId = serviceRecord.ServiceRecordId,
                VehicleId = serviceRecord.VehicleId,

                Plate = serviceRecord.Vehicle?.Plate ?? "-",

                CustomerName =
                    serviceRecord.Vehicle?.Customer?.FullName ?? "-",

                CheckInDate = serviceRecord.CheckInDate,

                EstimatedDeliveryDate =
                    serviceRecord.EstimatedDeliveryDate,

                ActualDeliveryDate =
                    serviceRecord.ActualDeliveryDate,

                Mileage = serviceRecord.Mileage,

                CustomerComplaint =
                    serviceRecord.CustomerComplaint,

                Description =
                    serviceRecord.Description,

                Status = serviceRecord.Status,

                TotalLaborPrice = totalLaborPrice,

                TotalPartsPrice = totalPartsPrice,

                TotalPrice =
                    totalLaborPrice + totalPartsPrice,

                TotalMechanicPayment =
                    totalMechanicPayment,

                TotalLaborGrossMargin =
                    totalLaborGrossMargin,
                AppointmentId =
                   serviceRecord.Appointment?
                       .AppointmentId,


            };
        }).ToList();
    }

    public async Task<ServiceRecordDetailDto?> GetByIdAsync(int id)
    {
        var value =
            await _serviceRecordRepository
                .GetByIdWithDetailsAsync(id);

        if (value == null ||
            value.IsDeleted)
        {
            return null;
        }

        var activeOperations = value.ServiceOperations
            .Where(x => !x.IsDeleted)
            .ToList();

        var totalLaborPrice = activeOperations
            .Sum(x => x.CustomerLaborPrice);

        var totalPartsPrice = activeOperations
            .SelectMany(x => x.ServiceOperationParts)
            .Where(x => !x.IsDeleted)
            .Sum(x => x.TotalPrice);

        var totalMechanicPayment = activeOperations
            .Sum(x => x.MechanicPayment);

        var totalLaborGrossMargin = activeOperations
            .Sum(x => x.LaborGrossMargin);

        return new ServiceRecordDetailDto
        {
            ServiceRecordId =
                value.ServiceRecordId,

            VehicleId =
                value.VehicleId,

            Plate =
                value.Vehicle?.Plate ?? "-",

            CustomerName =
                value.Vehicle?.Customer?.FullName ?? "-",

            CheckInDate =
                value.CheckInDate,

            EstimatedDeliveryDate =
                value.EstimatedDeliveryDate,

            ActualDeliveryDate =
                value.ActualDeliveryDate,

            Mileage =
                value.Mileage,

            Status =
                value.Status,

            CustomerComplaint =
                value.CustomerComplaint,

            Description =
                value.Description,

            FuelLevel =
                value.FuelLevel,

            ExistingDamages =
                value.ExistingDamages,

            DeliveredItems =
                value.DeliveredItems,

            AdvisorName =
                value.AdvisorName,

            CustomerNotes =
                value.CustomerNotes,

            VehicleDeliveredBy =
    value.VehicleDeliveredBy,

            VehicleDeliveredByPhone =
    value.VehicleDeliveredByPhone,

            PreApprovalLimit =
    value.PreApprovalLimit,

            RequiresApprovalForExtraWork =
    value.RequiresApprovalForExtraWork,

            ReturnOldPartsToCustomer =
    value.ReturnOldPartsToCustomer,

            TotalLaborPrice =
                totalLaborPrice,

            TotalPartsPrice =
                totalPartsPrice,

            TotalPrice =
                totalLaborPrice + totalPartsPrice,

            TotalMechanicPayment =
                totalMechanicPayment,

            TotalLaborGrossMargin =
                totalLaborGrossMargin,

            TotalOperationCount =
                activeOperations.Count,

            AppointmentId =
                 value.Appointment?
                 .AppointmentId,

            TotalPartCount =
                activeOperations
                    .SelectMany(x => x.ServiceOperationParts)
                    .Count(x => !x.IsDeleted)
        };
    }

    public async Task AddAsync(CreateServiceRecordDto dto)
    {
        if (dto.EstimatedDeliveryDate < dto.CheckInDate)
        {
            throw new Exception(
                "Tahmini teslim tarihi giriş tarihinden önce olamaz.");
        }

        var entity = new ServiceRecord
        {
            CheckInDate = dto.CheckInDate,
            EstimatedDeliveryDate = dto.EstimatedDeliveryDate,

            // Araç henüz teslim edilmediği için boş kalır.
            ActualDeliveryDate = null,

            Mileage = dto.Mileage,

            CustomerComplaint =
                dto.CustomerComplaint.Trim(),

            Description =
                dto.Description?.Trim(),
            FuelLevel =
                dto.FuelLevel,

            ExistingDamages =
                dto.ExistingDamages?.Trim(),

            DeliveredItems =
                  dto.DeliveredItems?.Trim(),

            AdvisorName =
                 dto.AdvisorName?.Trim(),

            CustomerNotes =
                 dto.CustomerNotes?.Trim(),
            VehicleDeliveredBy =
    dto.VehicleDeliveredBy?.Trim(),

            VehicleDeliveredByPhone =
    dto.VehicleDeliveredByPhone?.Trim(),

            PreApprovalLimit =
    dto.PreApprovalLimit,

            RequiresApprovalForExtraWork =
    dto.RequiresApprovalForExtraWork,

            ReturnOldPartsToCustomer =
    dto.ReturnOldPartsToCustomer,

            VehicleId = dto.VehicleId,

            // Servis ilk açıldığında enum'un ilk değeri kullanılır.
            Status = ServiceStatus.Bekliyor,

            CreatedDate = DateTime.Now,
            IsDeleted = false,

            /*
             * Eski kolonlar henüz veritabanından kaldırılmadığı için
             * geçici değerler veriyoruz.
             */
            LaborCost = 0,
            TotalPrice = 0
        };

        await _serviceRecordRepository.AddAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateServiceRecordDto dto)
    {
        var entity =
            await _serviceRecordRepository
                .GetByIdAsync(dto.ServiceRecordId);

        if (entity == null || entity.IsDeleted)
        {
            throw new Exception("Servis kaydı bulunamadı.");
        }

        if (dto.EstimatedDeliveryDate < dto.CheckInDate)
        {
            throw new Exception(
                "Tahmini teslim tarihi giriş tarihinden önce olamaz.");
        }

        var previousStatus = entity.Status;

        entity.CheckInDate = dto.CheckInDate;

        entity.EstimatedDeliveryDate =
            dto.EstimatedDeliveryDate;

        entity.Mileage = dto.Mileage;

        entity.CustomerComplaint =
            dto.CustomerComplaint.Trim();

        entity.Description =
            dto.Description?.Trim();
        entity.FuelLevel =
            dto.FuelLevel;

        entity.ExistingDamages =
            dto.ExistingDamages?.Trim();

        entity.DeliveredItems =
            dto.DeliveredItems?.Trim();

        entity.AdvisorName =
            dto.AdvisorName?.Trim();

        entity.CustomerNotes =
            dto.CustomerNotes?.Trim();

        entity.VehicleDeliveredBy =
            dto.VehicleDeliveredBy?.Trim();

        entity.VehicleDeliveredByPhone =
            dto.VehicleDeliveredByPhone?.Trim();

        entity.PreApprovalLimit =
            dto.PreApprovalLimit;

        entity.RequiresApprovalForExtraWork =
            dto.RequiresApprovalForExtraWork;

        entity.ReturnOldPartsToCustomer =
            dto.ReturnOldPartsToCustomer;

        entity.VehicleId = dto.VehicleId;

        entity.Status = dto.Status;

        entity.UpdatedDate = DateTime.Now;

        /*
         * Durum ilk kez teslim edildi aşamasına geçtiğinde
         * gerçek teslim tarihini otomatik doldurur.
         */
        if (IsDeliveredStatus(dto.Status) &&
            !IsDeliveredStatus(previousStatus) &&
            entity.ActualDeliveryDate == null)
        {
            entity.ActualDeliveryDate = DateTime.Now;
        }

        _serviceRecordRepository.Update(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity =
            await _serviceRecordRepository.GetByIdAsync(id);

        if (entity == null || entity.IsDeleted)
        {
            throw new Exception("Servis kaydı bulunamadı.");
        }

        await _serviceRecordRepository.SoftDeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeliverVehicleAsync(
    int serviceRecordId)
    {
        var entity =
            await _serviceRecordRepository
                .GetByIdWithDetailsAsync(serviceRecordId);

        if (entity == null || entity.IsDeleted)
        {
            throw new Exception(
                "Servis kaydı bulunamadı.");
        }

        if (entity.ActualDeliveryDate.HasValue)
        {
            throw new Exception(
                "Bu araç daha önce müşteriye teslim edilmiş.");
        }

        var activeOperations = entity.ServiceOperations
            .Where(x => !x.IsDeleted)
            .ToList();

        if (activeOperations.Count == 0)
        {
            throw new Exception(
                "Aracı teslim etmeden önce en az bir servis işlemi bulunmalıdır.");
        }

        var unfinishedOperations = activeOperations
            .Where(x =>
                x.Status != ServiceOperationStatus.Completed &&
                x.Status != ServiceOperationStatus.Cancelled)
            .ToList();

        if (unfinishedOperations.Count > 0)
        {
            throw new Exception(
                "Araç teslim edilemez. Tamamlanmamış servis işlemleri bulunuyor.");
        }

        var completedOperationCount = activeOperations.Count(x =>
            x.Status == ServiceOperationStatus.Completed);

        if (completedOperationCount == 0)
        {
            throw new Exception(
                "Teslim edilebilmesi için en az bir servis işlemi tamamlanmış olmalıdır.");
        }

        entity.Status =
            ServiceStatus.Tamamlandi;

        entity.ActualDeliveryDate =
            DateTime.Now;

        entity.UpdatedDate =
            DateTime.Now;

        _serviceRecordRepository.Update(entity);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<ServiceAcceptanceFormDto?>
    GetAcceptanceFormAsync(int id)
    {
        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdWithDetailsAsync(id);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            return null;
        }

        var systemSetting =
    await _systemSettingRepository
        .GetAsync();

        return new ServiceAcceptanceFormDto
        {
            ServiceRecordId =
                serviceRecord.ServiceRecordId,

            CheckInDate =
                serviceRecord.CheckInDate,

            EstimatedDeliveryDate =
                serviceRecord.EstimatedDeliveryDate,

            Mileage =
                serviceRecord.Mileage,

            Status =
                serviceRecord.Status,

            CustomerComplaint =
                serviceRecord.CustomerComplaint,

            Description =
                serviceRecord.Description,

            FuelLevel =
                serviceRecord.FuelLevel,

            ExistingDamages =
                serviceRecord.ExistingDamages,

            DeliveredItems =
                serviceRecord.DeliveredItems,

            AdvisorName =
                serviceRecord.AdvisorName,

            CustomerNotes =
                serviceRecord.CustomerNotes,

            VehicleDeliveredBy =
    serviceRecord.VehicleDeliveredBy,

            VehicleDeliveredByPhone =
    serviceRecord.VehicleDeliveredByPhone,

            PreApprovalLimit =
    serviceRecord.PreApprovalLimit,

            RequiresApprovalForExtraWork =
    serviceRecord.RequiresApprovalForExtraWork,

            ReturnOldPartsToCustomer =
    serviceRecord.ReturnOldPartsToCustomer,

            VehicleId =
                serviceRecord.VehicleId,

            Plate =
                serviceRecord.Vehicle?.Plate ?? "-",

            VinNumber =
                serviceRecord.Vehicle?.VinNumber ?? "-",

            ModelYear =
                serviceRecord.Vehicle?.ModelYear ?? 0,

            BrandName =
    serviceRecord.Vehicle?
        .Model?
        .Brand?
        .BrandName ?? "-",

            ModelName =
    serviceRecord.Vehicle?
        .Model?
        .ModelName ?? "-",

            CustomerId =
                serviceRecord.Vehicle?
                    .CustomerId ?? 0,

            CustomerName =
                serviceRecord.Vehicle?
                    .Customer?
                    .FullName ?? "-",

            CustomerPhone =
                serviceRecord.Vehicle?
                    .Customer?
                    .Phone ?? "-",

            CustomerEmail =
                serviceRecord.Vehicle?
                    .Customer?
                    .Email ?? "-",

            CustomerAddress =
                serviceRecord.Vehicle?
                    .Customer?
                    .Address ?? "-",

            CompanyName =
                systemSetting?.CompanyName ??
                "Auto Service",

            CompanyPhone =
                systemSetting?.CompanyPhone,

            CompanyEmail =
                systemSetting?.CompanyEmail,

            CompanyAddress =
                systemSetting?.CompanyAddress
        };
    }
    public async Task<ServiceExitReceiptDto?>
    GetExitReceiptAsync(int id)
    {
        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdWithDetailsAsync(id);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            return null;
        }

        var systemSetting =
            await _systemSettingRepository
                .GetAsync();

        var paymentEntities =
            await _paymentRepository
                .GetByServiceRecordIdAsync(id);

        var activeOperations =
            serviceRecord.ServiceOperations
                .Where(x => !x.IsDeleted)
                .ToList();

        var operationItems =
            activeOperations
                .Select(x =>
                    new ServiceExitOperationItemDto
                    {
                        ServiceOperationId =
                            x.ServiceOperationId,

                        OperationTypeName =
                            x.ServiceOperationType?.Name ?? "-",

                        MechanicFullName =
                            x.Mechanic == null
                                ? "-"
                                : $"{x.Mechanic.FirstName} {x.Mechanic.LastName}",

                        SpecialtyName =
                            x.Mechanic?
                                .MechanicSpecialty?
                                .Name ?? "-",

                        ProblemDescription =
                            x.ProblemDescription,

                        WorkDescription =
                            x.WorkDescription,

                        LaborHours =
                            x.LaborHours,

                        CustomerLaborPrice =
                            x.CustomerLaborPrice,

                        Status =
                            x.Status,

                        StartedDate =
                            x.StartedDate,

                        CompletedDate =
                            x.CompletedDate
                    })
                .ToList();

        var partItems =
            activeOperations
                .SelectMany(operation =>
                    operation.ServiceOperationParts
                        .Where(part => !part.IsDeleted)
                        .Select(part =>
                            new ServiceExitPartItemDto
                            {
                                ServiceOperationPartId =
                                    part.ServiceOperationPartId,

                                ServiceOperationId =
                                    operation.ServiceOperationId,

                                OperationTypeName =
                                    operation.ServiceOperationType?.Name ?? "-",

                                SparePartId =
                                    part.SparePartId,

                                PartName =
                                    part.SparePart?.PartName ?? "-",

                                PartCode =
                                    part.SparePart?.PartCode ?? "-",

                                Quantity =
                                    part.Quantity,

                                UnitPrice =
                                    part.UnitPrice,

                                TotalPrice =
                                    part.TotalPrice
                            }))
                .ToList();

        var laborSubtotal =
    operationItems.Sum(x =>
        x.CustomerLaborPrice);

        var partsSubtotal =
            partItems.Sum(x =>
                x.TotalPrice);

        /*
         * Sistemde girilen fiyatlar KDV DAHİLDİR.
         */
        var grandTotal =
            laborSubtotal +
            partsSubtotal;

        var vatRate =
            systemSetting?.VatRate ?? 20;

        decimal vatAmount = 0;
        decimal subtotal = grandTotal;

        if (vatRate > 0)
        {
            vatAmount =
                grandTotal -
                (grandTotal / (1 + vatRate / 100));

            subtotal =
                grandTotal -
                vatAmount;
        }
        var paymentItems =
    paymentEntities
        .Select(x => new ResultPaymentDto
        {
            PaymentId =
                x.PaymentId,

            ServiceRecordId =
                x.ServiceRecordId,

            Amount =
                x.Amount,

            PaymentMethod =
                x.PaymentMethod,

            PaymentDate =
                x.PaymentDate,

            Description =
                x.Description,

            TransactionReference =
                x.TransactionReference
        })
        .OrderByDescending(x =>
            x.PaymentDate)
        .ToList();

        var totalPaid =
            paymentItems.Sum(x =>
                x.Amount);

        var remainingAmount =
            grandTotal -
            totalPaid;

        if (remainingAmount < 0)
        {
            remainingAmount = 0;
        }

        var isFullyPaid =
            grandTotal > 0 &&
            remainingAmount <= 0;

        var hasPartialPayment =
            totalPaid > 0 &&
            remainingAmount > 0;

        var paymentStatusText =
            isFullyPaid
                ? "Borç Kapandı"
                : hasPartialPayment
                    ? "Kısmi Ödeme"
                    : "Ödeme Bekleniyor";
        return new ServiceExitReceiptDto
        {
            ServiceRecordId =
                serviceRecord.ServiceRecordId,

            CheckInDate =
                serviceRecord.CheckInDate,

            EstimatedDeliveryDate =
                serviceRecord.EstimatedDeliveryDate,

            ActualDeliveryDate =
                serviceRecord.ActualDeliveryDate,

            Mileage =
                serviceRecord.Mileage,

            Status =
                serviceRecord.Status,

            Description =
                serviceRecord.Description,

            AdvisorName =
                serviceRecord.AdvisorName,

            CustomerName =
                serviceRecord.Vehicle?
                    .Customer?
                    .FullName ?? "-",

            CustomerPhone =
                serviceRecord.Vehicle?
                    .Customer?
                    .Phone ?? "-",

            CustomerEmail =
                serviceRecord.Vehicle?
                    .Customer?
                    .Email ?? "-",

            CustomerAddress =
                serviceRecord.Vehicle?
                    .Customer?
                    .Address ?? "-",

            Plate =
                serviceRecord.Vehicle?
                    .Plate ?? "-",

            VinNumber =
                serviceRecord.Vehicle?
                    .VinNumber ?? "-",

            ModelYear =
                serviceRecord.Vehicle?
                    .ModelYear ?? 0,

            BrandName =
                serviceRecord.Vehicle?
                    .Model?
                    .Brand?
                    .BrandName ?? "-",

            ModelName =
                serviceRecord.Vehicle?
                    .Model?
                    .ModelName ?? "-",

            CompanyName =
                systemSetting?.CompanyName ??
                "Auto Service",

            CompanyPhone =
                systemSetting?.CompanyPhone,

            CompanyEmail =
                systemSetting?.CompanyEmail,

            CompanyAddress =
                systemSetting?.CompanyAddress,

            VatRate =
                vatRate,

            Currency =
                systemSetting?.Currency ??
                "TRY",

            Operations =
                operationItems,

            Parts =
                partItems,

            LaborSubtotal =
                laborSubtotal,

            PartsSubtotal =
                partsSubtotal,

            Subtotal =
                subtotal,

            VatAmount =
                vatAmount,

            GrandTotal =
                grandTotal,

            TotalPaid =
                totalPaid,

            RemainingAmount =
                remainingAmount,

            PaymentStatusText =
                paymentStatusText,

            IsFullyPaid =
                isFullyPaid,

            HasPartialPayment =
                hasPartialPayment,

            Payments =
                paymentItems,

            TotalOperationCount =
                operationItems.Count,

            TotalPartQuantity =
                partItems.Sum(x => x.Quantity)
        };
    }
    private static bool IsDeliveredStatus(ServiceStatus status)
    {
        var statusName = status.ToString();

        return statusName.Equals(
                   "Delivered",
                   StringComparison.OrdinalIgnoreCase) ||
               statusName.Equals(
                   "TeslimEdildi",
                   StringComparison.OrdinalIgnoreCase) ||
               statusName.Equals(
                   "DeliveredToCustomer",
                   StringComparison.OrdinalIgnoreCase);
    }
}