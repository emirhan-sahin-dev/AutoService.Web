using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.AppointmentDtos;
using AutoService.Entity.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentService
        _appointmentService;

    private readonly ICustomerRepository
        _customerRepository;

    private readonly IVehicleRepository
        _vehicleRepository;

    public AppointmentController(
        IAppointmentService appointmentService,
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository)
    {
        _appointmentService =
            appointmentService;

        _customerRepository =
            customerRepository;

        _vehicleRepository =
            vehicleRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var values =
            await _appointmentService
                .GetAllAsync();

        return View(values);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(
        int id)
    {
        var value =
            await _appointmentService
                .GetByIdAsync(id);

        if (value == null)
        {
            TempData["Error"] =
                "Randevu bulunamadı.";

            return RedirectToAction(
                nameof(Index));
        }

        return View(value);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            new CreateAppointmentDto
            {
                AppointmentDate =
                    DateTime.Now
                        .AddDays(1)
                        .Date
                        .AddHours(9),

                Status =
                    AppointmentStatus.Waiting
            };

        await LoadDropdownsAsync(
            dto.CustomerId,
            dto.VehicleId,
            dto.Status);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateAppointmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(
                dto.CustomerId,
                dto.VehicleId,
                dto.Status);

            return View(dto);
        }

        try
        {
            await _appointmentService
                .AddAsync(dto);

            TempData["Success"] =
                "Randevu başarıyla oluşturuldu.";

            return RedirectToAction(
                nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            await LoadDropdownsAsync(
                dto.CustomerId,
                dto.VehicleId,
                dto.Status);

            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Update(
        int id)
    {
        var value =
            await _appointmentService
                .GetByIdAsync(id);

        if (value == null)
        {
            TempData["Error"] =
                "Randevu bulunamadı.";

            return RedirectToAction(
                nameof(Index));
        }

        if (value.ServiceRecordId.HasValue)
        {
            TempData["Error"] =
                "Servis kaydına dönüştürülmüş randevu güncellenemez.";

            return RedirectToAction(
                nameof(Detail),
                new
                {
                    id
                });
        }

        var dto =
            new UpdateAppointmentDto
            {
                AppointmentId =
                    value.AppointmentId,

                CustomerId =
                    value.CustomerId,

                VehicleId =
                    value.VehicleId,

                AppointmentDate =
                    value.AppointmentDate,

                CustomerRequest =
                    value.CustomerRequest,

                Description =
                    value.Description,

                Status =
                    value.Status
            };

        await LoadDropdownsAsync(
            dto.CustomerId,
            dto.VehicleId,
            dto.Status);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        UpdateAppointmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(
                dto.CustomerId,
                dto.VehicleId,
                dto.Status);

            return View(dto);
        }

        try
        {
            await _appointmentService
                .UpdateAsync(dto);

            TempData["Success"] =
                "Randevu başarıyla güncellendi.";

            return RedirectToAction(
                nameof(Detail),
                new
                {
                    id = dto.AppointmentId
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                ex.Message);

            await LoadDropdownsAsync(
                dto.CustomerId,
                dto.VehicleId,
                dto.Status);

            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await _appointmentService
                .DeleteAsync(id);

            TempData["Success"] =
                "Randevu silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;
        }

        return RedirectToAction(
            nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult>
        GetVehiclesByCustomer(
            int customerId)
    {
        if (customerId <= 0)
        {
            return Json(
                Array.Empty<object>());
        }

        var vehicles =
            await _vehicleRepository
                .FindAsync(x =>
                    x.CustomerId ==
                        customerId &&
                    !x.IsDeleted);

        var result =
            vehicles
                .OrderBy(x =>
                    x.Plate)
                .Select(x => new
                {
                    id =
                        x.VehicleId,

                    text =
                        $"{x.Plate} - " +
                        $"{x.ModelYear}"
                })
                .ToList();

        return Json(result);
    }

    private async Task LoadDropdownsAsync(
        int? selectedCustomerId = null,
        int? selectedVehicleId = null,
        AppointmentStatus? selectedStatus = null)
    {
        var customers =
            await _customerRepository
                .GetAllAsync();

        var vehicles =
            await _vehicleRepository
                .GetAllAsync();

        ViewBag.Customers =
            new SelectList(
                customers
                    .Where(x =>
                        !x.IsDeleted)
                    .OrderBy(x =>
                        x.FullName),
                "CustomerId",
                "FullName",
                selectedCustomerId);

        var filteredVehicles =
            selectedCustomerId.HasValue &&
            selectedCustomerId.Value > 0
                ? vehicles
                    .Where(x =>
                        !x.IsDeleted &&
                        x.CustomerId ==
                            selectedCustomerId.Value)
                    .OrderBy(x =>
                        x.Plate)
                    .ToList()
                : new List<AutoService.Entity.Entities.Vehicle>();

        ViewBag.Vehicles =
            new SelectList(
                filteredVehicles,
                "VehicleId",
                "Plate",
                selectedVehicleId);

        ViewBag.Statuses =
            Enum.GetValues<AppointmentStatus>()
                .Select(status =>
                    new SelectListItem
                    {
                        Value =
                            ((int)status)
                                .ToString(),

                        Text =
                            GetStatusText(status),

                        Selected =
                            selectedStatus.HasValue &&
                            selectedStatus.Value ==
                                status
                    })
                .ToList();
    }

    private static string GetStatusText(
        AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Waiting =>
                "Bekliyor",

            AppointmentStatus.Confirmed =>
                "Onaylandı",

            AppointmentStatus.Arrived =>
                "Servise Geldi",

            AppointmentStatus.Completed =>
                "Tamamlandı",

            AppointmentStatus.Cancelled =>
                "İptal Edildi",

            _ => status.ToString()
        };
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConvertToService(
    int id)
    {
        try
        {
            var serviceRecordId =
                await _appointmentService
                    .ConvertToServiceRecordAsync(id);

            TempData["Success"] =
                "Randevu servis kaydına dönüştürüldü.";

            return RedirectToAction(
                "Detail",
                "ServiceRecord",
                new
                {
                    id = serviceRecordId
                });
        }
        catch (Exception ex)
        {
            TempData["Error"] =
                ex.Message;

            return RedirectToAction(
                nameof(Detail),
                new
                {
                    id
                });
        }
    }
}