using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.DataAccess.Repositories.Concrete;
using AutoService.Dto.ServiceOperationDtos;
using AutoService.Entity.Entities;
using AutoService.Entity.Enums;

namespace AutoService.Business.Services.Concrete;

public class ServiceOperationManager : IServiceOperationService
{
    private readonly IServiceOperationRepository _serviceOperationRepository;
    private readonly IServiceOperationTypeRepository _serviceOperationTypeRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IServiceRecordRepository _serviceRecordRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceOperationPartRepository
    _serviceOperationPartRepository;

    private readonly ISparePartRepository
        _sparePartRepository;

    public ServiceOperationManager(
    IServiceOperationRepository serviceOperationRepository,
    IServiceOperationTypeRepository serviceOperationTypeRepository,
    IMechanicRepository mechanicRepository,
    IServiceRecordRepository serviceRecordRepository,
    IServiceOperationPartRepository serviceOperationPartRepository,
    ISparePartRepository sparePartRepository,
    IUnitOfWork unitOfWork)
    {
        _serviceOperationRepository =
            serviceOperationRepository;

        _serviceOperationTypeRepository =
            serviceOperationTypeRepository;

        _mechanicRepository =
            mechanicRepository;

        _serviceRecordRepository =
            serviceRecordRepository;

        _serviceOperationPartRepository =
            serviceOperationPartRepository;

        _sparePartRepository =
            sparePartRepository;

        _unitOfWork =
            unitOfWork;
    }

    public async Task<List<ResultServiceOperationDto>> GetAllAsync()
    {
        var values =
            await _serviceOperationRepository
                .GetOperationsWithDetailsAsync();

        return values.Select(x =>
        {
            var partsTotal = x.ServiceOperationParts?
                .Where(y => !y.IsDeleted)
                .Sum(y => y.TotalPrice) ?? 0;

            return new ResultServiceOperationDto
            {
                ServiceOperationId = x.ServiceOperationId,
                ServiceRecordId = x.ServiceRecordId,

                VehiclePlate =
                    x.ServiceRecord?.Vehicle?.Plate ?? "-",

                OperationTypeName =
                    x.ServiceOperationType?.Name ?? "-",

                SpecialtyName =
                    x.ServiceOperationType?
                        .MechanicSpecialty?.Name ?? "-",

                MechanicId = x.MechanicId,

                MechanicFullName =
                    $"{x.Mechanic?.FirstName} " +
                    $"{x.Mechanic?.LastName}".Trim(),

                LaborHours = x.LaborHours,

                CustomerLaborPrice =
                    x.CustomerLaborPrice,

                MechanicPayment =
                    x.MechanicPayment,

                LaborGrossMargin =
                    x.LaborGrossMargin,

                PartsTotal =
                    partsTotal,

                CustomerTotal =
                    x.CustomerLaborPrice + partsTotal,

                Status =
                    x.Status,

                CreatedDate =
                    x.CreatedDate
            };
        }).ToList();
    }

    public async Task<List<ResultServiceOperationDto>>
        GetByServiceRecordIdAsync(int serviceRecordId)
    {
        var values =
            await _serviceOperationRepository
                .GetOperationsByServiceRecordAsync(
                    serviceRecordId);

        return values.Select(x =>
        {
            var partsTotal = x.ServiceOperationParts?
                .Where(y => !y.IsDeleted)
                .Sum(y => y.TotalPrice) ?? 0;

            return new ResultServiceOperationDto
            {
                ServiceOperationId =
                    x.ServiceOperationId,

                ServiceRecordId =
                    x.ServiceRecordId,

                VehiclePlate =
                    x.ServiceRecord?.Vehicle?.Plate ?? "-",

                OperationTypeName =
                    x.ServiceOperationType?.Name ?? "-",

                SpecialtyName =
                    x.ServiceOperationType?
                        .MechanicSpecialty?.Name ?? "-",

                MechanicId =
                    x.MechanicId,

                MechanicFullName =
                    $"{x.Mechanic?.FirstName} " +
                    $"{x.Mechanic?.LastName}".Trim(),

                LaborHours =
                    x.LaborHours,

                CustomerLaborPrice =
                    x.CustomerLaborPrice,

                MechanicPayment =
                    x.MechanicPayment,

                LaborGrossMargin =
                    x.LaborGrossMargin,

                PartsTotal =
                    partsTotal,

                CustomerTotal =
                    x.CustomerLaborPrice + partsTotal,

                Status =
                    x.Status,

                CreatedDate =
                    x.CreatedDate
            };
        }).ToList();
    }

    public async Task<ServiceOperationDetailDto?> GetByIdAsync(int id)
    {
        var value =
            await _serviceOperationRepository
                .GetOperationWithPartsAsync(id);

        if (value == null)
        {
            return null;
        }

        var parts = value.ServiceOperationParts?
            .Where(x => !x.IsDeleted)
            .Select(x => new ServiceOperationPartItemDto
            {
                ServiceOperationPartId =
                    x.ServiceOperationPartId,

                SparePartId =
                    x.SparePartId,

                PartName =
                    x.SparePart?.PartName ?? "-",

                PartCode =
                    x.SparePart?.PartCode ?? "-",

                Quantity =
                    x.Quantity,

                UnitPrice =
                    x.UnitPrice,

                TotalPrice =
                    x.TotalPrice
            })
            .ToList()
            ?? new List<ServiceOperationPartItemDto>();

        var partsTotal =
            parts.Sum(x => x.TotalPrice);

        return new ServiceOperationDetailDto
        {
            ServiceOperationId =
                value.ServiceOperationId,

            ServiceRecordId =
                value.ServiceRecordId,

            VehiclePlate =
                value.ServiceRecord?.Vehicle?.Plate ?? "-",

            OperationTypeName =
                value.ServiceOperationType?.Name ?? "-",

            SpecialtyName =
                value.ServiceOperationType?
                    .MechanicSpecialty?.Name ?? "-",

            MechanicId =
                value.MechanicId,

            MechanicFullName =
                $"{value.Mechanic?.FirstName} " +
                $"{value.Mechanic?.LastName}".Trim(),

            ProblemDescription =
                value.ProblemDescription,

            WorkDescription =
                value.WorkDescription,

            LaborHours =
                value.LaborHours,

            CustomerLaborPrice =
                value.CustomerLaborPrice,

            MechanicPayment =
                value.MechanicPayment,

            LaborGrossMargin =
                value.LaborGrossMargin,

            PartsTotal =
                partsTotal,

            CustomerTotal =
                value.CustomerLaborPrice + partsTotal,

            Status =
                value.Status,

            StartedDate =
                value.StartedDate,

            CompletedDate =
                value.CompletedDate,

            CreatedDate =
                value.CreatedDate,

            Parts =
                parts
        };
    }

    public async Task AddAsync(
        CreateServiceOperationDto dto)
    {
        var operationType =
            await _serviceOperationTypeRepository
                .GetByIdWithSpecialtyAsync(
                    dto.ServiceOperationTypeId);

        if (operationType == null)
        {
            throw new Exception(
                "İşlem türü bulunamadı.");
        }

        var mechanic =
            await _mechanicRepository
                .GetByIdAsync(dto.MechanicId);

        if (mechanic == null ||
            mechanic.IsDeleted ||
            !mechanic.IsActive)
        {
            throw new Exception(
                "Seçilen usta bulunamadı veya aktif değil.");
        }

        if (mechanic.MechanicSpecialtyId !=
            operationType.MechanicSpecialtyId)
        {
            throw new Exception(
                "Seçilen usta bu işlem türünün " +
                "uzmanlığına uygun değil.");
        }

        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdAsync(dto.ServiceRecordId);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            throw new Exception(
                "Servis kaydı bulunamadı.");
        }

        var serviceOperation = new ServiceOperation
        {
            ServiceRecordId =
                dto.ServiceRecordId,

            ServiceOperationTypeId =
                dto.ServiceOperationTypeId,

            MechanicId =
                dto.MechanicId,

            ProblemDescription =
                dto.ProblemDescription.Trim(),

            WorkDescription =
                dto.WorkDescription?.Trim(),

            LaborHours =
                operationType.DefaultDurationHours,

            CustomerLaborPrice =
                operationType.CustomerLaborPrice,

            MechanicPayment =
                operationType.MechanicPayment,

            LaborGrossMargin =
                operationType.CustomerLaborPrice -
                operationType.MechanicPayment,

            Status =
                ServiceOperationStatus.Waiting,

            CreatedDate =
                DateTime.Now,

            IsDeleted =
                false
        };

        await _serviceOperationRepository
            .AddAsync(serviceOperation);

        // Önce yeni işlemi veritabanına kaydeder.
        await _unitOfWork.SaveChangesAsync();

        // Ardından ana servis kaydının durumunu günceller.
        await SyncServiceRecordStatusAsync(
            dto.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddBatchAsync(
    CreateServiceOperationBatchDto dto)
    {
        if (dto.Operations == null ||
            dto.Operations.Count == 0)
        {
            throw new Exception(
                "En az bir servis işlemi eklemelisiniz.");
        }

        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdAsync(dto.ServiceRecordId);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            throw new Exception(
                "Servis kaydı bulunamadı.");
        }

        /*
         * Bütün işlem satırlarındaki parça taleplerini toplar.
         * Aynı parça farklı işlemlerde seçilmiş olabilir.
         */
        var requestedPartQuantities = dto.Operations
            .Where(x => x.Parts != null)
            .SelectMany(x => x.Parts)
            .Where(x =>
                x.SparePartId > 0 &&
                x.Quantity > 0)
            .GroupBy(x => x.SparePartId)
            .ToDictionary(
                x => x.Key,
                x => x.Sum(y => y.Quantity));

        /*
         * Kullanılacak yedek parçaları bir kez yükler
         * ve toplam stok yeterliliğini kontrol eder.
         */
        var sparePartDictionary =
            new Dictionary<int, SparePart>();

        foreach (var requestedPart in requestedPartQuantities)
        {
            var sparePart =
                await _sparePartRepository
                    .GetByIdAsync(requestedPart.Key);

            if (sparePart == null ||
                sparePart.IsDeleted)
            {
                throw new Exception(
                    $"Seçilen yedek parçalardan biri bulunamadı. " +
                    $"Parça ID: {requestedPart.Key}");
            }

            if (sparePart.StockQuantity <
                requestedPart.Value)
            {
                throw new Exception(
                    $"{sparePart.PartName} için yeterli stok yok. " +
                    $"Mevcut stok: {sparePart.StockQuantity}, " +
                    $"istenen adet: {requestedPart.Value}");
            }

            sparePartDictionary.Add(
                sparePart.SparePartId,
                sparePart);
        }

        foreach (var item in dto.Operations)
        {
            var operationType =
                await _serviceOperationTypeRepository
                    .GetByIdWithSpecialtyAsync(
                        item.ServiceOperationTypeId);

            if (operationType == null ||
                operationType.IsDeleted ||
                !operationType.IsActive)
            {
                throw new Exception(
                    "Seçilen işlem türlerinden biri bulunamadı veya aktif değil.");
            }

            var mechanic =
                await _mechanicRepository
                    .GetByIdAsync(item.MechanicId);

            if (mechanic == null ||
                mechanic.IsDeleted ||
                !mechanic.IsActive)
            {
                throw new Exception(
                    "Seçilen ustalardan biri bulunamadı veya aktif değil.");
            }

            if (mechanic.MechanicSpecialtyId !=
                operationType.MechanicSpecialtyId)
            {
                throw new Exception(
                    $"{mechanic.FirstName} {mechanic.LastName}, " +
                    $"{operationType.Name} işlemi için " +
                    "uygun uzmanlığa sahip değil.");
            }

            var serviceOperation =
                new ServiceOperation
                {
                    ServiceRecordId =
                        dto.ServiceRecordId,

                    ServiceOperationTypeId =
                        item.ServiceOperationTypeId,

                    MechanicId =
                        item.MechanicId,

                    ProblemDescription =
                        item.ProblemDescription.Trim(),

                    WorkDescription =
                        item.WorkDescription?.Trim(),

                    LaborHours =
                        operationType.DefaultDurationHours,

                    CustomerLaborPrice =
                        operationType.CustomerLaborPrice,

                    MechanicPayment =
                        operationType.MechanicPayment,

                    LaborGrossMargin =
                        operationType.CustomerLaborPrice -
                        operationType.MechanicPayment,

                    Status =
                        ServiceOperationStatus.Waiting,

                    CreatedDate =
                        DateTime.Now,

                    IsDeleted =
                        false
                };

            /*
             * Aynı işlem satırında aynı parça birden fazla
             * seçildiyse adetlerini birleştirir.
             */
            var operationParts = item.Parts?
    .Where(x =>
        x.SparePartId > 0 &&
        x.Quantity > 0)
    .GroupBy(x => x.SparePartId)
    .Select(x => (
        SparePartId: x.Key,
        Quantity: x.Sum(y => y.Quantity)))
    .ToList()
    ?? new List<(int SparePartId, int Quantity)>();

            foreach (var partItem in operationParts)
            {
                if (!sparePartDictionary.TryGetValue(
                        partItem.SparePartId,
                        out var sparePart))
                {
                    throw new Exception(
                        "Seçilen yedek parça bulunamadı.");
                }

                var operationPart =
                    new ServiceOperationPart
                    {
                        SparePartId =
                            sparePart.SparePartId,

                        Quantity =
                            partItem.Quantity,

                        UnitPrice =
                            sparePart.UnitPrice,

                        TotalPrice =
                            sparePart.UnitPrice *
                            partItem.Quantity,

                        CreatedDate =
                            DateTime.Now,

                        IsDeleted =
                            false
                    };

                serviceOperation
                    .ServiceOperationParts
                    .Add(operationPart);
            }

            await _serviceOperationRepository
                .AddAsync(serviceOperation);
        }

        /*
         * Bütün işlemlerde kullanılan toplam adetleri
         * stoktan düşer.
         */
        foreach (var requestedPart in requestedPartQuantities)
        {
            var sparePart =
                sparePartDictionary[requestedPart.Key];

            sparePart.StockQuantity -=
                requestedPart.Value;

            _sparePartRepository.Update(
                sparePart);
        }

        /*
         * İşlemler, parçalar ve stok değişiklikleri
         * aynı SaveChanges çağrısıyla kaydedilir.
         */
        await _unitOfWork.SaveChangesAsync();

        await SyncServiceRecordStatusAsync(
            dto.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(
        UpdateServiceOperationStatusDto dto)
    {
        var value =
            await _serviceOperationRepository
                .GetByIdAsync(dto.ServiceOperationId);

        if (value == null || value.IsDeleted)
        {
            throw new Exception(
                "Servis işlemi bulunamadı.");
        }

        value.Status =
            dto.Status;

        value.WorkDescription =
            dto.WorkDescription?.Trim();

        value.UpdatedDate =
            DateTime.Now;

        if (dto.Status ==
                ServiceOperationStatus.InProgress &&
            value.StartedDate == null)
        {
            value.StartedDate =
                DateTime.Now;
        }

        if (dto.Status ==
                ServiceOperationStatus.Completed &&
            value.CompletedDate == null)
        {
            value.CompletedDate =
                DateTime.Now;
        }

        // Tamamlandı durumundan çıkılırsa eski bitiş tarihi temizlenir.
        if (dto.Status !=
                ServiceOperationStatus.Completed)
        {
            value.CompletedDate = null;
        }

        _serviceOperationRepository.Update(value);

        await _unitOfWork.SaveChangesAsync();

        // İşlem durumu değişince ana servis kaydını günceller.
        await SyncServiceRecordStatusAsync(
            value.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var value =
            await _serviceOperationRepository
                .GetByIdAsync(id);

        if (value == null || value.IsDeleted)
        {
            throw new Exception(
                "Servis işlemi bulunamadı.");
        }

        var serviceRecordId =
            value.ServiceRecordId;

        value.IsDeleted =
            true;

        value.DeletedDate =
            DateTime.Now;

        value.UpdatedDate =
            DateTime.Now;

        _serviceOperationRepository.Update(value);

        await _unitOfWork.SaveChangesAsync();

        // İşlem silinince kalan işlemlere göre ana durumu hesaplar.
        await SyncServiceRecordStatusAsync(
            serviceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task AddPartAsync(
    AddServiceOperationPartDto dto)
    {
        var operation =
            await _serviceOperationRepository
                .GetByIdAsync(dto.ServiceOperationId);

        if (operation == null ||
            operation.IsDeleted)
        {
            throw new Exception(
                "Servis işlemi bulunamadı.");
        }

        var sparePart =
            await _sparePartRepository
                .GetByIdAsync(dto.SparePartId);

        if (sparePart == null ||
            sparePart.IsDeleted)
        {
            throw new Exception(
                "Yedek parça bulunamadı.");
        }

        if (dto.Quantity <= 0)
        {
            throw new Exception(
                "Parça adedi en az 1 olmalıdır.");
        }

        if (sparePart.StockQuantity < dto.Quantity)
        {
            throw new Exception(
                $"Yetersiz stok. Mevcut stok: " +
                $"{sparePart.StockQuantity}");
        }

        var existingPart =
            await _serviceOperationPartRepository
                .GetByOperationAndSparePartAsync(
                    dto.ServiceOperationId,
                    dto.SparePartId);

        if (existingPart != null)
        {
            existingPart.Quantity +=
                dto.Quantity;

            existingPart.UnitPrice =
                sparePart.UnitPrice;

            existingPart.TotalPrice =
                existingPart.Quantity *
                existingPart.UnitPrice;

            _serviceOperationPartRepository
                .Update(existingPart);
        }
        else
        {
            var deletedPart =
                await _serviceOperationPartRepository
                    .GetDeletedByOperationAndSparePartAsync(
                        dto.ServiceOperationId,
                        dto.SparePartId);

            if (deletedPart != null)
            {
                deletedPart.IsDeleted =
                    false;

                deletedPart.DeletedDate =
                    null;

                deletedPart.Quantity =
                    dto.Quantity;

                deletedPart.UnitPrice =
                    sparePart.UnitPrice;

                deletedPart.TotalPrice =
                    dto.Quantity *
                    sparePart.UnitPrice;

                _serviceOperationPartRepository
                    .Update(deletedPart);
            }
            else
            {
                var operationPart =
                    new ServiceOperationPart
                    {
                        ServiceOperationId =
                            dto.ServiceOperationId,

                        SparePartId =
                            dto.SparePartId,

                        Quantity =
                            dto.Quantity,

                        UnitPrice =
                            sparePart.UnitPrice,

                        TotalPrice =
                            dto.Quantity *
                            sparePart.UnitPrice,

                        CreatedDate =
                            DateTime.Now,

                        IsDeleted =
                            false
                    };

                await _serviceOperationPartRepository
                    .AddAsync(operationPart);
            }
        }

        sparePart.StockQuantity -=
            dto.Quantity;

        _sparePartRepository.Update(
            sparePart);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemovePartAsync(
        int serviceOperationPartId)
    {
        var operationPart =
            await _serviceOperationPartRepository
                .GetByIdWithDetailsAsync(
                    serviceOperationPartId);

        if (operationPart == null ||
            operationPart.IsDeleted)
        {
            throw new Exception(
                "Servis işlemine ait parça kaydı bulunamadı.");
        }

        var sparePart =
            operationPart.SparePart;

        if (sparePart == null ||
            sparePart.IsDeleted)
        {
            throw new Exception(
                "Yedek parça bulunamadı.");
        }

        sparePart.StockQuantity +=
            operationPart.Quantity;

        _sparePartRepository.Update(
            sparePart);

        await _serviceOperationPartRepository
            .SoftDeleteAsync(operationPart);

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task SyncServiceRecordStatusAsync(
    int serviceRecordId)
    {
        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdAsync(serviceRecordId);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            return;
        }

        /*
         * Araç müşteriye teslim edilmişse işlem durumları
         * artık ana servis kaydının durumunu değiştiremez.
         *
         * ServiceStatus enumunda şu an "TeslimEdildi" değeri
         * bulunmadığı için durum Tamamlandi olarak korunur.
         */
        if (serviceRecord.ActualDeliveryDate.HasValue)
        {
            serviceRecord.Status =
                ServiceStatus.Tamamlandi;

            serviceRecord.UpdatedDate =
                DateTime.Now;

            _serviceRecordRepository.Update(
                serviceRecord);

            return;
        }

        var operations =
            await _serviceOperationRepository
                .GetOperationsByServiceRecordAsync(
                    serviceRecordId);

        var activeOperations = operations
            .Where(x => !x.IsDeleted)
            .ToList();

        /*
         * İptal edilen işlemler, tamamlanma hesabının
         * dışında tutulur.
         */
        var nonCancelledOperations = activeOperations
            .Where(x =>
                x.Status !=
                ServiceOperationStatus.Cancelled)
            .ToList();

        ServiceStatus newStatus;

        /*
         * Henüz servis işlemi eklenmediyse
         * servis kaydı bekliyor olarak kalır.
         */
        if (activeOperations.Count == 0)
        {
            newStatus =
                ServiceStatus.Bekliyor;
        }

        /*
         * Bütün işlemler iptal edilmişse
         * ana servis kaydı da iptal edilir.
         */
        else if (activeOperations.All(x =>
                     x.Status ==
                     ServiceOperationStatus.Cancelled))
        {
            newStatus =
                ServiceStatus.IptalEdildi;
        }

        /*
         * Bir işlem bile parça bekliyorsa,
         * öncelikli durum parça bekleniyordur.
         */
        else if (nonCancelledOperations.Any(x =>
                     x.Status ==
                     ServiceOperationStatus.WaitingForPart))
        {
            newStatus =
                ServiceStatus.ParcaBekleniyor;
        }

        /*
         * Bir işlem işleme alınmışsa
         * ana servis kaydı işlemde görünür.
         */
        else if (nonCancelledOperations.Any(x =>
                     x.Status ==
                     ServiceOperationStatus.InProgress))
        {
            newStatus =
                ServiceStatus.Islemde;
        }

        /*
         * Bir işlem kalite kontrol aşamasındaysa
         * ServiceStatus enumunda ayrı bir kalite kontrol
         * değeri bulunmadığı için işlemde kabul edilir.
         */
        else if (nonCancelledOperations.Any(x =>
                     x.Status ==
                     ServiceOperationStatus.QualityControl))
        {
            newStatus =
                ServiceStatus.Islemde;
        }

        /*
         * İptal edilmemiş bütün işlemler tamamlandıysa
         * ana servis kaydı tamamlandı olur.
         *
         * Burada ActualDeliveryDate doldurulmaz.
         * Gerçek teslim tarihi yalnızca araç müşteriye
         * teslim edildiğinde ayrı işlemle atanacaktır.
         */
        else if (nonCancelledOperations.Count > 0 &&
                 nonCancelledOperations.All(x =>
                     x.Status ==
                         ServiceOperationStatus.Completed ||
                     x.Status ==
                         ServiceOperationStatus.ReadyForDelivery))
        {
            newStatus =
                ServiceStatus.Tamamlandi;
        }

        /*
         * En az bir işlem tamamlandı fakat diğer işlemler
         * henüz bitmediyse servis hâlâ işlemde kabul edilir.
         */
        else if (nonCancelledOperations.Any(x =>
                     x.Status ==
                     ServiceOperationStatus.Completed))
        {
            newStatus =
                ServiceStatus.Islemde;
        }

        /*
         * İşlemler henüz başlamadıysa bekliyor durumundadır.
         */
        else
        {
            newStatus =
                ServiceStatus.Bekliyor;
        }

        serviceRecord.Status =
            newStatus;

        serviceRecord.UpdatedDate =
            DateTime.Now;

        _serviceRecordRepository.Update(
            serviceRecord);
    }

    /*
     * ServiceStatus enumundaki isimler projeden projeye
     * farklı olabileceği için birden fazla isim dener.
     *
     * Hiçbiri bulunamazsa mevcut durum korunur.
     */
    private static ServiceStatus FindServiceStatus(
        ServiceStatus currentStatus,
        params string[] possibleNames)
    {
        foreach (var name in possibleNames)
        {
            if (Enum.TryParse<ServiceStatus>(
                    name,
                    true,
                    out var status))
            {
                return status;
            }
        }

        return currentStatus;
    }
}