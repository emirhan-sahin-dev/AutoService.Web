using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.ServiceOperationDtos;
using AutoService.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers;

[Authorize]
public class ServiceOperationController : Controller
{
    private readonly IServiceOperationService _serviceOperationService;
    private readonly IServiceOperationTypeRepository _operationTypeRepository;
    private readonly IServiceRecordRepository _serviceRecordRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly ISparePartService _sparePartService;
    private readonly ISparePartRepository _sparePartRepository;
    private readonly IServiceOperationTypeSparePartRepository
    _serviceOperationTypeSparePartRepository;

    public ServiceOperationController(
     IServiceOperationService serviceOperationService,
     IServiceOperationTypeRepository operationTypeRepository,
     IServiceRecordRepository serviceRecordRepository,
     IMechanicRepository mechanicRepository,
     ISparePartService sparePartService,
     ISparePartRepository sparePartRepository,
     IServiceOperationTypeSparePartRepository
    serviceOperationTypeSparePartRepository)
    {
        _serviceOperationService =
            serviceOperationService;

        _operationTypeRepository =
            operationTypeRepository;

        _serviceRecordRepository =
            serviceRecordRepository;

        _mechanicRepository =
            mechanicRepository;

        _sparePartService =
            sparePartService;

        _sparePartRepository = 
            sparePartRepository;

        _serviceOperationTypeSparePartRepository =
    serviceOperationTypeSparePartRepository;
    }

    // Tüm servis işlemlerini veya seçili servis kaydının işlemlerini listeler.
    [HttpGet]
    public async Task<IActionResult> Index(
    int? serviceRecordId)
    {
        List<ResultServiceOperationDto> values;

        if (serviceRecordId.HasValue &&
            serviceRecordId.Value > 0)
        {
            values =
                await _serviceOperationService
                    .GetByServiceRecordIdAsync(
                        serviceRecordId.Value);

            ViewBag.ServiceRecordId =
                serviceRecordId.Value;
        }
        else
        {
            values =
                await _serviceOperationService
                    .GetAllAsync();

            ViewBag.ServiceRecordId =
                null;
        }

        return View(values);
    }

    // Tek servis işlemi ekleme sayfasını açar.
    [HttpGet]
    public async Task<IActionResult> Create(
    int? serviceRecordId)
    {
        if (!serviceRecordId.HasValue ||
            serviceRecordId.Value <= 0)
        {
            TempData["Error"] =
                "İşlem eklemek için önce bir servis kaydı seçmelisiniz.";

            return RedirectToAction(
                "Index",
                "ServiceRecord");
        }

        var serviceRecord =
            await _serviceRecordRepository
                .GetByIdAsync(serviceRecordId.Value);

        if (serviceRecord == null ||
            serviceRecord.IsDeleted)
        {
            TempData["Error"] =
                "Seçilen servis kaydı bulunamadı.";

            return RedirectToAction(
                "Index",
                "ServiceRecord");
        }

        var dto = new CreateServiceOperationDto
        {
            ServiceRecordId =
                serviceRecordId.Value
        };

        ViewBag.ServiceRecordId =
            serviceRecordId.Value;

        ViewBag.ReturnServiceRecordId =
            serviceRecordId.Value;

        await LoadDropdownsAsync(
            serviceRecordId.Value);

        return View(dto);
    }

    // Tek servis işlemini kaydeder.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateServiceOperationDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnServiceRecordId =
                dto.ServiceRecordId > 0
                  ? (int?)dto.ServiceRecordId
                 : null;

            await LoadDropdownsAsync(
                dto.ServiceRecordId,
                dto.ServiceOperationTypeId);

            return View(dto);
        }

        try
        {
            await _serviceOperationService.AddAsync(dto);

            TempData["Success"] =
                "Servis işlemi başarıyla eklendi.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    serviceRecordId = dto.ServiceRecordId,
                    refresh = DateTime.UtcNow.Ticks
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            ViewBag.ReturnServiceRecordId =
    dto.ServiceRecordId > 0
        ? (int?)dto.ServiceRecordId
        : null;

            await LoadDropdownsAsync(
                dto.ServiceRecordId,
                dto.ServiceOperationTypeId);

            return View(dto);
        }
    }

    // Servis işleminin detaylarını gösterir.
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value =
            await _serviceOperationService.GetByIdAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        ViewBag.ServiceRecordId =
            value.ServiceRecordId;

        return View(value);
    }
    [HttpGet]
    public async Task<IActionResult> AddPart(
    int serviceOperationId)
    {
        var operation =
            await _serviceOperationService
                .GetByIdAsync(serviceOperationId);

        if (operation == null)
        {
            TempData["Error"] =
                "Servis işlemi bulunamadı.";

            return RedirectToAction(nameof(Index));
        }

        var spareParts =
            await _sparePartService.GetAllAsync();

        ViewBag.SpareParts =
            new SelectList(
                spareParts
                    .Where(x => x.StockQuantity > 0)
                    .OrderBy(x => x.PartName)
                    .Select(x => new
                    {
                        x.SparePartId,

                        DisplayText =
                            $"{x.PartName} - " +
                            $"{x.PartCode} - " +
                            $"Stok: {x.StockQuantity} - " +
                            $"{x.UnitPrice:N2} ₺"
                    }),
                "SparePartId",
                "DisplayText");

        ViewBag.OperationTypeName =
            operation.OperationTypeName;

        ViewBag.VehiclePlate =
            operation.VehiclePlate;

        ViewBag.ServiceRecordId =
            operation.ServiceRecordId;

        var dto =
            new AddServiceOperationPartDto
            {
                ServiceOperationId =
                    serviceOperationId,

                Quantity = 1
            };

        return View(dto);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPart(
    AddServiceOperationPartDto dto)
    {
        if (!ModelState.IsValid)
        {
            await ReloadAddPartViewBagsAsync(
                dto.ServiceOperationId,
                dto.SparePartId);

            return View(dto);
        }

        try
        {
            await _serviceOperationService
                .AddPartAsync(dto);

            TempData["Success"] =
                "Yedek parça servis işlemine eklendi ve stoktan düşüldü.";

            return RedirectToAction(
                nameof(Detail),
                new
                {
                    id = dto.ServiceOperationId
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            await ReloadAddPartViewBagsAsync(
                dto.ServiceOperationId,
                dto.SparePartId);

            return View(dto);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePart(
    int serviceOperationPartId,
    int serviceOperationId)
    {
        try
        {
            await _serviceOperationService
                .RemovePartAsync(
                    serviceOperationPartId);

            TempData["Success"] =
                "Yedek parça işlemden kaldırıldı ve stok miktarı geri eklendi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Detail),
            new
            {
                id = serviceOperationId
            });
    }

    // Durum güncelleme formunu açar.
    [HttpGet]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var value =
            await _serviceOperationService
                .GetByIdAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        SetOperationViewBags(value);

        ViewBag.ServiceRecordId =
            value.ServiceRecordId;

        LoadStatuses();

        var dto = new UpdateServiceOperationStatusDto
        {
            ServiceOperationId =
                value.ServiceOperationId,

            Status =
                value.Status,

            WorkDescription =
                value.WorkDescription
        };

        return View(dto);
    }

    // Servis işleminin durumunu günceller.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(
        UpdateServiceOperationStatusDto dto)
    {
        if (!ModelState.IsValid)
        {
            await ReloadOperationViewBagsAsync(
                dto.ServiceOperationId);

            LoadStatuses();

            return View(dto);
        }

        try
        {
            await _serviceOperationService
                .UpdateStatusAsync(dto);

            var updatedOperation =
                await _serviceOperationService
                    .GetByIdAsync(dto.ServiceOperationId);

            TempData["Success"] =
                "İşlem durumu başarıyla güncellendi.";

            if (updatedOperation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(
                nameof(Index),
                new
                {
                    serviceRecordId =
                        updatedOperation.ServiceRecordId
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            await ReloadOperationViewBagsAsync(
                dto.ServiceOperationId);

            LoadStatuses();

            return View(dto);
        }
    }

    // Servis işlemini soft delete yapar.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int id,
        int? serviceRecordId = null)
    {
        try
        {
            await _serviceOperationService.DeleteAsync(id);

            TempData["Success"] =
                "Servis işlemi başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(
            nameof(Index),
            new
            {
                serviceRecordId,
                refresh = DateTime.UtcNow.Ticks
            });
    }

    // Çoklu servis işlemi ekleme sayfasını açar.
    [HttpGet]
    public async Task<IActionResult> CreateMultiple(
        int? serviceRecordId = null)
    {
        var validServiceRecordId =
            serviceRecordId.HasValue &&
            serviceRecordId.Value > 0
                ? serviceRecordId
                : null;

        if (validServiceRecordId.HasValue)
        {
            var serviceRecord =
                await _serviceRecordRepository.GetByIdAsync(
                    validServiceRecordId.Value);

            if (serviceRecord == null ||
                serviceRecord.IsDeleted)
            {
                TempData["Error"] =
                    "Seçilen servis kaydı bulunamadı.";

                return RedirectToAction(
                    nameof(Index));
            }
        }

        var dto = new CreateServiceOperationBatchDto
        {
            ServiceRecordId =
                validServiceRecordId ?? 0,

            Operations =
    new List<CreateServiceOperationItemDto>
    {
        new()
        {
            Parts =
                new List<CreateServiceOperationPartItemDto>
                {
                    new()
                }
        }
    }



        };

        ViewBag.ReturnServiceRecordId =
            validServiceRecordId;

        await LoadDropdownsAsync(
            validServiceRecordId);

        return View(dto);
    }

    // Birden fazla servis işlemini tek seferde kaydeder.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMultiple(
        CreateServiceOperationBatchDto dto)
    {
        dto.Operations ??=
            new List<CreateServiceOperationItemDto>();

        // Tamamen boş bırakılan satırları temizler.
        dto.Operations = dto.Operations
            .Where(x =>
                x.ServiceOperationTypeId > 0 ||
                x.MechanicId > 0 ||
                !string.IsNullOrWhiteSpace(
                    x.ProblemDescription) ||
                !string.IsNullOrWhiteSpace(
                    x.WorkDescription))
            .ToList();

        /*
         * Formdan gelen eski indeksli doğrulama kayıtlarını
         * temizleyip güncel DTO'yu yeniden doğrular.
         */
        ModelState.Clear();
        TryValidateModel(dto);

        if (dto.ServiceRecordId <= 0)
        {
            ModelState.AddModelError(
                nameof(dto.ServiceRecordId),
                "Servis kaydı seçiniz.");
        }

        if (dto.Operations.Count == 0)
        {
            ModelState.AddModelError(
                nameof(dto.Operations),
                "En az bir servis işlemi eklemelisiniz.");
        }

        for (var i = 0;
             i < dto.Operations.Count;
             i++)
        {
            var item = dto.Operations[i];

            if (item.ServiceOperationTypeId <= 0)
            {
                ModelState.AddModelError(
                    $"Operations[{i}].ServiceOperationTypeId",
                    $"{i + 1}. işlem için işlem türü seçiniz.");
            }

            if (item.MechanicId <= 0)
            {
                ModelState.AddModelError(
                    $"Operations[{i}].MechanicId",
                    $"{i + 1}. işlem için usta seçiniz.");
            }

            if (string.IsNullOrWhiteSpace(
                    item.ProblemDescription))
            {
                ModelState.AddModelError(
                    $"Operations[{i}].ProblemDescription",
                    $"{i + 1}. işlem için problem açıklaması yazınız.");
            }
        }

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnServiceRecordId =
    dto.ServiceRecordId > 0
        ? (int?)dto.ServiceRecordId
        : null;

            await LoadDropdownsAsync(
                dto.ServiceRecordId > 0
                    ? dto.ServiceRecordId
                    : null);

            return View(dto);
        }

        try
        {
            await _serviceOperationService
                .AddBatchAsync(dto);

            TempData["Success"] =
                $"{dto.Operations.Count} servis işlemi başarıyla eklendi.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    serviceRecordId = dto.ServiceRecordId,
                    refresh = DateTime.UtcNow.Ticks
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            ViewBag.ReturnServiceRecordId =
    dto.ServiceRecordId > 0
        ? (int?)dto.ServiceRecordId
        : null;

            await LoadDropdownsAsync(
                dto.ServiceRecordId > 0
                    ? dto.ServiceRecordId
                    : null);

            return View(dto);
        }
    }

    /*
     * Seçilen işlem türünün uzmanlığına uygun
     * aktif ustaları JSON olarak gönderir.
     */
    [HttpGet]
    public async Task<IActionResult>
        GetMechanicsByOperationType(
            int operationTypeId)
    {
        if (operationTypeId <= 0)
        {
            return Json(Array.Empty<object>());
        }

        var operationType =
            await _operationTypeRepository
                .GetByIdWithSpecialtyAsync(
                    operationTypeId);

        if (operationType == null)
        {
            return Json(Array.Empty<object>());
        }

        var mechanics =
            await _mechanicRepository
                .GetMechanicsBySpecialtyIdAsync(
                    operationType.MechanicSpecialtyId);

        var result = mechanics.Select(x => new
        {
            id = x.MechanicId,

            name =
                $"{x.FirstName} {x.LastName}",

            specialty =
                x.MechanicSpecialty?.Name ?? "-"
        });

        return Json(result);
    }

    /*
     * İşlem türünün varsayılan süre ve
     * fiyat bilgilerini JSON olarak gönderir.
     */
    [HttpGet]
    public async Task<IActionResult>
        GetOperationTypeInfo(
            int operationTypeId)
    {
        if (operationTypeId <= 0)
        {
            return BadRequest();
        }

        var operationType =
            await _operationTypeRepository
                .GetByIdWithSpecialtyAsync(
                    operationTypeId);

        if (operationType == null)
        {
            return NotFound();
        }

        return Json(new
        {
            durationHours =
                operationType.DefaultDurationHours,

            customerLaborPrice =
                operationType.CustomerLaborPrice,

            mechanicPayment =
                operationType.MechanicPayment,

            laborGrossMargin =
                operationType.CustomerLaborPrice -
                operationType.MechanicPayment,

            specialtyId =
                operationType.MechanicSpecialtyId,

            specialtyName =
                operationType.MechanicSpecialty?.Name ?? "-"
        });
    }

    // Servis kaydı ve işlem türü dropdownlarını hazırlar.
    private async Task LoadDropdownsAsync(
    int? selectedServiceRecordId = null,
    int? selectedOperationTypeId = null)
    {
        var serviceRecords =
            await _serviceRecordRepository
                .GetAllAsync();

        var operationTypes =
            await _operationTypeRepository
                .GetAllWithSpecialtyAsync();

        var spareParts =
            await _sparePartRepository
                .GetAllAsync();

        var activeServiceRecords = serviceRecords
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.ServiceRecordId)
            .Select(x => new
            {
                x.ServiceRecordId,

                DisplayText =
                    $"Servis #{x.ServiceRecordId}"
            })
            .ToList();

        ViewBag.ServiceRecords =
            new SelectList(
                activeServiceRecords,
                "ServiceRecordId",
                "DisplayText",
                selectedServiceRecordId);

        ViewBag.OperationTypes =
            new SelectList(
                operationTypes
                    .Where(x =>
                        !x.IsDeleted &&
                        x.IsActive)
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        x.ServiceOperationTypeId,

                        DisplayText =
                            $"{x.Name} - " +
                            $"{x.MechanicSpecialty?.Name ?? "-"}"
                    })
                    .ToList(),
                "ServiceOperationTypeId",
                "DisplayText",
                selectedOperationTypeId);

        ViewBag.SpareParts =
            spareParts
                .Where(x =>
                    !x.IsDeleted &&
                    x.StockQuantity > 0)
                .OrderBy(x => x.PartName)
                .Select(x => new
                {
                    x.SparePartId,
                    x.PartName,
                    x.PartCode,
                    x.UnitPrice,
                    x.StockQuantity,

                    DisplayText =
                        $"{x.PartName} - {x.PartCode} " +
                        $"| Stok: {x.StockQuantity} " +
                        $"| {x.UnitPrice:N2} ₺"
                })
                .ToList();

        ViewBag.ServiceRecordId =
            selectedServiceRecordId;

        ViewBag.ReturnServiceRecordId ??=
            selectedServiceRecordId.HasValue &&
            selectedServiceRecordId.Value > 0
                ? selectedServiceRecordId
                : null;
    }

    // UpdateStatus ekranındaki özet bilgileri tekrar yükler.
    private async Task ReloadOperationViewBagsAsync(
    int serviceOperationId)
    {
        var value =
            await _serviceOperationService
                .GetByIdAsync(serviceOperationId);

        if (value != null)
        {
            SetOperationViewBags(value);

            ViewBag.ServiceRecordId =
                value.ServiceRecordId;
        }
    }

    private void SetOperationViewBags(
        ServiceOperationDetailDto value)
    {
        ViewBag.OperationTypeName =
            value.OperationTypeName;

        ViewBag.VehiclePlate =
            value.VehiclePlate;

        ViewBag.MechanicFullName =
            value.MechanicFullName;
    }

    private static string GetStatusText(
        ServiceOperationStatus status)
    {
        return status switch
        {
            ServiceOperationStatus.Waiting =>
                "Bekliyor",

            ServiceOperationStatus.InProgress =>
                "İşleme Alındı",

            ServiceOperationStatus.WaitingForPart =>
                "Parça Bekleniyor",

            ServiceOperationStatus.Completed =>
                "Tamamlandı",

            ServiceOperationStatus.QualityControl =>
                "Kalite Kontrol",

            ServiceOperationStatus.ReadyForDelivery =>
                "Teslime Hazır",

            ServiceOperationStatus.Cancelled =>
                "İptal Edildi",

            _ => status.ToString()
        };
    }
    private async Task ReloadAddPartViewBagsAsync(
    int serviceOperationId,
    int? selectedSparePartId = null)
    {
        var operation =
            await _serviceOperationService
                .GetByIdAsync(serviceOperationId);

        if (operation != null)
        {
            ViewBag.OperationTypeName =
                operation.OperationTypeName;

            ViewBag.VehiclePlate =
                operation.VehiclePlate;

            ViewBag.ServiceRecordId =
                operation.ServiceRecordId;
        }

        var spareParts =
            await _sparePartService.GetAllAsync();

        ViewBag.SpareParts =
            new SelectList(
                spareParts
                    .Where(x =>
                        x.StockQuantity > 0 ||
                        x.SparePartId ==
                            selectedSparePartId)
                    .OrderBy(x => x.PartName)
                    .Select(x => new
                    {
                        x.SparePartId,

                        DisplayText =
                            $"{x.PartName} - " +
                            $"{x.PartCode} - " +
                            $"Stok: {x.StockQuantity} - " +
                            $"{x.UnitPrice:N2} ₺"
                    }),
                "SparePartId",
                "DisplayText",
                selectedSparePartId);
    }
    private void LoadStatuses()
    {
        ViewBag.Statuses = Enum
            .GetValues<ServiceOperationStatus>()
            .Select(status => new SelectListItem
            {
                Value = ((int)status).ToString(),
                Text = GetStatusText(status)
            })
            .ToList();
    }
    [HttpGet]
    public async Task<IActionResult> GetSparePartsByOperationType(
    int operationTypeId)
    {
        if (operationTypeId <= 0)
        {
            return Json(Array.Empty<object>());
        }

        var values =
            await _serviceOperationTypeSparePartRepository
                .GetByOperationTypeIdAsync(operationTypeId);

        var result = values
            .Where(x =>
                x.SparePart != null &&
                !x.SparePart.IsDeleted &&
                x.SparePart.StockQuantity > 0)
            .Select(x => new
            {
                id = x.SparePartId,

                name = x.SparePart.PartName,

                code = x.SparePart.PartCode,

                price = x.SparePart.UnitPrice,

                stock = x.SparePart.StockQuantity,

                text =
                    $"{x.SparePart.PartName} - " +
                    $"{x.SparePart.PartCode} | " +
                    $"Stok: {x.SparePart.StockQuantity} | " +
                    $"{x.SparePart.UnitPrice:N2} ₺"
            })
            .OrderBy(x => x.name)
            .ToList();

        return Json(result);
    }
}