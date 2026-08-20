using AutoService.Web.Security;
using AutoService.Business.Services.Abstract;
using AutoService.Dto.ServiceDetailDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AllStaff)]
public class ServiceDetailController : Controller
{
    private readonly IServiceDetailService _serviceDetailService;
    private readonly IServiceRecordService _serviceRecordService;
    private readonly ISparePartService _sparePartService;

    public ServiceDetailController(
        IServiceDetailService serviceDetailService,
        IServiceRecordService serviceRecordService,
        ISparePartService sparePartService)
    {
        _serviceDetailService = serviceDetailService;
        _serviceRecordService = serviceRecordService;
        _sparePartService = sparePartService;
    }

    public async Task<IActionResult> Index(
     string? keyword,
     int page = 1)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var serviceDetails =
            await _serviceDetailService.GetAllAsync();

        var query = serviceDetails.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.Plate) &&
                 x.Plate.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.SparePartName) &&
                 x.SparePartName.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase))
            );
        }

        var totalCount = query.Count();

        var totalPage = (int)Math.Ceiling(
            totalCount / (double)pageSize
        );

        if (totalPage == 0)
        {
            totalPage = 1;
        }

        if (page > totalPage)
        {
            page = totalPage;
        }

        var values = query
            .OrderByDescending(x => x.ServiceDetailId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Keyword = keyword;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPage = totalPage;
        ViewBag.TotalCount = totalCount;

        return View(values);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value = await _serviceDetailService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        return View(value);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Create()
    {
        ViewBag.ServiceRecords = await _serviceRecordService.GetAllAsync();
        ViewBag.SpareParts = await _sparePartService.GetAllAsync();

        return View();
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Create(CreateServiceDetailDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ServiceRecords = await _serviceRecordService.GetAllAsync();
            ViewBag.SpareParts = await _sparePartService.GetAllAsync();

            return View(dto);
        }

        await _serviceDetailService.AddAsync(dto);

        TempData["Success"] = "Servis detayı başarıyla oluşturuldu.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Update(int id)
    {
        var value = await _serviceDetailService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        ViewBag.ServiceRecords = await _serviceRecordService.GetAllAsync();
        ViewBag.SpareParts = await _sparePartService.GetAllAsync();

        var dto = new UpdateServiceDetailDto
        {
            ServiceDetailId = value.ServiceDetailId,
            ServiceRecordId = value.ServiceRecordId,
            SparePartId = value.SparePartId,
            Quantity = value.Quantity,
            UnitPrice = value.UnitPrice,
            TotalPrice = value.TotalPrice
        };

        return View(dto);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public async Task<IActionResult> Update(UpdateServiceDetailDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ServiceRecords = await _serviceRecordService.GetAllAsync();
            ViewBag.SpareParts = await _sparePartService.GetAllAsync();

            return View(dto);
        }

        await _serviceDetailService.UpdateAsync(dto);

        TempData["Success"] = "Servis Detayı Başarıyla Güncellendi. ";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _serviceDetailService.DeleteAsync(id);

        TempData["Success"] =
            "Servis detayı başarıyla silindi.";

        return RedirectToAction(nameof(Index));
    }
}
