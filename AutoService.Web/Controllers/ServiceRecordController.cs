using AutoService.Business.Services.Abstract;
using AutoService.Dto.ServiceRecordDtos;
using AutoService.Entity.Enums;
using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AllStaff)]
public class ServiceRecordController : Controller
{
    private readonly IServiceRecordService _serviceRecordService;
    private readonly IVehicleService _vehicleService;

    public ServiceRecordController(
        IServiceRecordService serviceRecordService,
        IVehicleService vehicleService)
    {
        _serviceRecordService = serviceRecordService;
        _vehicleService = vehicleService;
    }

    // Servis kayıtlarını listeler, arama ve sayfalama yapar.
    [HttpGet]
    public async Task<IActionResult> Index(
        string? keyword,
        int page = 1)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var serviceRecords =
            await _serviceRecordService.GetAllAsync();

        var query = serviceRecords.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.Plate) &&
                 x.Plate.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.CustomerName) &&
                 x.CustomerName.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase))
            );
        }

        var totalCount = query.Count();

        var totalPage = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        if (totalPage == 0)
        {
            totalPage = 1;
        }

        if (page > totalPage)
        {
            page = totalPage;
        }

        var values = query
            .OrderByDescending(x => x.CheckInDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Keyword = keyword;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPage = totalPage;
        ViewBag.TotalCount = totalCount;

        return View(values);
    }

    // Yeni servis kabul formunu açar.
    [HttpGet]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Create()
    {
        await LoadVehiclesAsync();

        var dto = new CreateServiceRecordDto
        {
            CheckInDate = DateTime.Now,
            EstimatedDeliveryDate = DateTime.Now.AddDays(1)
        };

        return View(dto);
    }

    // Yeni servis kaydını oluşturur.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Create(
        CreateServiceRecordDto dto)
    {
        ValidateDates(
            dto.CheckInDate,
            dto.EstimatedDeliveryDate,
            nameof(dto.EstimatedDeliveryDate));

        if (!ModelState.IsValid)
        {
            await LoadVehiclesAsync(dto.VehicleId);

            return View(dto);
        }

        try
        {
            await _serviceRecordService.AddAsync(dto);

            TempData["Success"] =
                "Servis kaydı başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            await LoadVehiclesAsync(dto.VehicleId);

            return View(dto);
        }
    }

    // Servis kaydının detaylarını gösterir.
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value =
            await _serviceRecordService.GetByIdAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        return View(value);
    }
    [HttpGet]
    public async Task<IActionResult> AcceptanceForm(int id)
    {
        var value =
            await _serviceRecordService
                .GetAcceptanceFormAsync(id);

        if (value == null)
        {
            TempData["Error"] =
                "Servis kabul formu için kayıt bulunamadı.";

            return RedirectToAction(nameof(Index));
        }

        return View(value);
    }
    [HttpGet]
    public async Task<IActionResult> ExitReceipt(int id)
    {
        var value =
            await _serviceRecordService
                .GetExitReceiptAsync(id);

        if (value == null)
        {
            TempData["Error"] =
                "Servis çıkış fişi için kayıt bulunamadı.";

            return RedirectToAction(nameof(Index));
        }

        return View(value);
    }
    // Servis kaydı güncelleme formunu açar.
    [HttpGet]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Update(int id)
    {
        var value =
            await _serviceRecordService.GetByIdAsync(id);

        if (value == null)
        {
            return NotFound();
        }

        await LoadVehiclesAsync(value.VehicleId);
        LoadStatuses(value.Status);

        var dto = new UpdateServiceRecordDto
        {
            ServiceRecordId = value.ServiceRecordId,
            CheckInDate = value.CheckInDate,

            EstimatedDeliveryDate =
                value.EstimatedDeliveryDate ??
                value.CheckInDate.AddDays(1),

            ActualDeliveryDate =
                value.ActualDeliveryDate,

            Mileage = value.Mileage,
            CustomerComplaint = value.CustomerComplaint,
            Description = value.Description,
            FuelLevel = value.FuelLevel,
            ExistingDamages =value.ExistingDamages,
            DeliveredItems =value.DeliveredItems,
            AdvisorName =value.AdvisorName,
            CustomerNotes =value.CustomerNotes,
            VehicleDeliveredBy =value.VehicleDeliveredBy,
            VehicleDeliveredByPhone = value.VehicleDeliveredByPhone,
            PreApprovalLimit = value.PreApprovalLimit,
            RequiresApprovalForExtraWork =value.RequiresApprovalForExtraWork,
            ReturnOldPartsToCustomer = value.ReturnOldPartsToCustomer,
            Status = value.Status,
            VehicleId = value.VehicleId
        };

        return View(dto);
    }

    // Servis kaydını günceller.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Update(
        UpdateServiceRecordDto dto)
    {
        ValidateDates(
            dto.CheckInDate,
            dto.EstimatedDeliveryDate,
            nameof(dto.EstimatedDeliveryDate));

        if (!ModelState.IsValid)
        {
            await LoadVehiclesAsync(dto.VehicleId);
            LoadStatuses(dto.Status);

            return View(dto);
        }

        try
        {
            await _serviceRecordService.UpdateAsync(dto);

            TempData["Success"] =
                "Servis kaydı başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            await LoadVehiclesAsync(dto.VehicleId);
            LoadStatuses(dto.Status);

            return View(dto);
        }
    }

    // Servis kaydını soft delete yapar.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _serviceRecordService.DeleteAsync(id);

            TempData["Success"] =
                "Servis kaydı başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> DeliverVehicle(int id)
    {
        try
        {
            await _serviceRecordService
                .DeliverVehicleAsync(id);

            TempData["Success"] =
                "Araç müşteriye başarıyla teslim edildi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Index));
    }
    // Araç dropdown listesini hazırlar.
    private async Task LoadVehiclesAsync(
        int? selectedVehicleId = null)
    {
        var vehicles =
            await _vehicleService.GetAllAsync();

        ViewBag.Vehicles = new SelectList(
            vehicles.Select(x => new
            {
                x.VehicleId,

                DisplayText =
                    string.IsNullOrWhiteSpace(x.CustomerName)
                        ? x.Plate
                        : $"{x.Plate} - {x.CustomerName}"
            }),
            "VehicleId",
            "DisplayText",
            selectedVehicleId);
    }

    // Servis durumlarını dropdown için hazırlar.
    private void LoadStatuses(
        ServiceStatus? selectedStatus = null)
    {
        ViewBag.Statuses = Enum
            .GetValues<ServiceStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = GetStatusText(x),
                Selected =
                    selectedStatus.HasValue &&
                    selectedStatus.Value == x
            })
            .ToList();
    }

    // Tahmini teslim tarihi kontrolü.
    private void ValidateDates(
        DateTime checkInDate,
        DateTime estimatedDeliveryDate,
        string propertyName)
    {
        if (estimatedDeliveryDate < checkInDate)
        {
            ModelState.AddModelError(
                propertyName,
                "Tahmini teslim tarihi giriş tarihinden önce olamaz.");
        }
    }

    // Enum durumlarını Türkçe gösterir.
    private static string GetStatusText(
        ServiceStatus status)
    {
        return status.ToString();
    }
}