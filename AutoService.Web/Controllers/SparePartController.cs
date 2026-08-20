using AutoService.Web.Security;
using AutoService.Business.Services.Abstract;
using AutoService.Dto.SparePartDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
public class SparePartController : Controller
{
    private readonly ISparePartService _sparePartService;

    public SparePartController(ISparePartService sparePartService)
    {
        _sparePartService = sparePartService;
    }

    public async Task<IActionResult> Index(
      string? keyword,
      int page = 1,
      long? refresh = null)
    {
        const int pageSize = 8;

        if (page < 1)
        {
            page = 1;
        }

        var spareParts = await _sparePartService.GetAllAsync();

        var query = spareParts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.PartName) &&
                 x.PartName.Contains(
                     keyword,
                     StringComparison.OrdinalIgnoreCase)) ||

                (!string.IsNullOrWhiteSpace(x.PartCode) &&
                 x.PartCode.Contains(
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
            .OrderByDescending(x => x.SparePartId)
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
    public async Task<IActionResult> Create(CreateSparePartDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _sparePartService.AddAsync(dto);

        TempData["Success"] = "Yedek parça başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var value = await _sparePartService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        return View(value);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var value = await _sparePartService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        var dto = new UpdateSparePartDto
        {
            SparePartId = value.SparePartId,
            PartName = value.PartName,
            PartCode = value.PartCode,
            UnitPrice = value.UnitPrice,
            StockQuantity = value.StockQuantity
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
       UpdateSparePartDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            await _sparePartService.UpdateAsync(dto);

            TempData["Success"] =
                "Yedek parça başarıyla güncellendi.";

            return RedirectToAction(
                "Index",
                "SparePart",
                new
                {
                    refresh = DateTime.UtcNow.Ticks
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _sparePartService.DeleteAsync(id);

        TempData["Success"] =
            "Yedek parça başarıyla silindi.";

        return RedirectToAction(nameof(Index));
    }
}