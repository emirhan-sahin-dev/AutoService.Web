using AutoService.Web.Security;
using AutoService.Business.Services.Abstract;
using AutoService.Dto.MechanicDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
public class MechanicController : Controller
{
    private readonly IMechanicService _mechanicService;

    public MechanicController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
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

        var mechanics = await _mechanicService.GetAllAsync();

        var query = mechanics.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.FullName) &&
                 x.FullName.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.Specialty) &&
                 x.Specialty.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.Phone) &&
                 x.Phone.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.Email) &&
                 x.Email.Contains(
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
            .OrderBy(x => x.FullName)
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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMechanicDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _mechanicService.AddAsync(dto);

        TempData["Success"] = "Usta başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value = await _mechanicService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        return View(value);
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var value = await _mechanicService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        var dto = new UpdateMechanicDto
        {
            MechanicId = value.MechanicId,
            FirstName = value.FirstName,
            LastName = value.LastName,
            Phone = value.Phone,
            Email = value.Email,
            Specialty = value.Specialty,
            HireDate = value.HireDate
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateMechanicDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _mechanicService.UpdateAsync(dto);

        TempData["Success"] = "Usta başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _mechanicService.DeleteAsync(id);

        TempData["Success"] =
            "Usta başarıyla silindi.";

        return RedirectToAction(nameof(Index));
    }
}
