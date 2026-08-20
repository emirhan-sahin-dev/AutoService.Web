using AutoService.Business.Services.Abstract;
using AutoService.Dto.RoleDtos;
using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.Admin)]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _roleService.GetAllAsync();

        return View(values);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _roleService.AddAsync(dto);

            TempData["Success"] = "Rol başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return View(dto);
        }
    }

    public async Task<IActionResult> Update(int id)
    {
        var value = await _roleService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        var dto = new UpdateRoleDto
        {
            RoleId = value.RoleId,
            RoleName = value.RoleName
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateRoleDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _roleService.UpdateAsync(dto);

            TempData["Success"] = "Rol güncellendi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;

            return View(dto);
        }
    }

    public async Task<IActionResult> Detail(int id)
    {
        var value = await _roleService.GetByIdAsync(id);

        if (value == null)
            return NotFound();

        return View(value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _roleService.DeleteAsync(id);

            TempData["Success"] = "Rol silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}