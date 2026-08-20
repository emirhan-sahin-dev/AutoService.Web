using AutoService.Web.Security;
using Microsoft.AspNetCore.Mvc;
using AutoService.Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using AutoService.Dto.VehicleDtos;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
public class VehicleController : Controller
{
    private readonly IVehicleService _vehicleService;
    private readonly ICustomerService _customerService;
    private readonly IBrandService _brandService;
    private readonly IModelService _modelService;

    public VehicleController(
        IVehicleService vehicleService,
        ICustomerService customerService,
        IBrandService brandService,
        IModelService modelService)
    {
        _vehicleService = vehicleService;
        _customerService = customerService;
        _brandService = brandService;
        _modelService = modelService;

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

        var vehicles = await _vehicleService.GetAllAsync();

        var query = vehicles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.Plate) &&
                 x.Plate.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.BrandName) &&
                 x.BrandName.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.ModelName) &&
                 x.ModelName.Contains(
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
            .OrderByDescending(x => x.VehicleId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Keyword = keyword;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPage = totalPage;
        ViewBag.TotalCount = totalCount;
        ViewBag.PageSize = pageSize;

        return View(values);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Customers = await _vehicleService.GetCustomersAsync();
        ViewBag.Brands = await _vehicleService.GetBrandsAsync();
        ViewBag.Models = await _vehicleService.GetModelsAsync();

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await _customerService.GetAllAsync();
            ViewBag.Brands = await _brandService.GetAllAsync();
            ViewBag.Models = await _modelService.GetAllAsync();

            return View(dto);
        }

        await _vehicleService.AddAsync(dto);

        TempData["Success"] = "Araç başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var value = await _vehicleService.GetUpdateDtoAsync(id);

        if (value == null)
            return NotFound();

        ViewBag.Customers = await _vehicleService.GetCustomersAsync();
        ViewBag.Brands = await _vehicleService.GetBrandsAsync();
        ViewBag.Models = await _vehicleService.GetModelsAsync();

        return View(value);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateVehicleDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await _vehicleService.GetCustomersAsync();
            ViewBag.Brands = await _vehicleService.GetBrandsAsync();
            ViewBag.Models = await _vehicleService.GetModelsAsync();

            return View(dto);
        }

        await _vehicleService.UpdateAsync(dto);

        TempData["Success"] = "Araç başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        await _vehicleService.DeleteAsync(id);

        TempData["Success"] = "Araç başarıyla silindi.";

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value = await _vehicleService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        return View(value);
    }
}
