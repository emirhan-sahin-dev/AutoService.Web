using AutoService.Business.Services.Abstract;
using AutoService.Dto.UserDtos;
using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.Admin)]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UserController(
        IUserService userService,
        IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index(
       string? keyword = null,
       int page = 1,
       int pageSize = 10)
    {
        if (page < 1)
            page = 1;

        var result = await _userService.GetPagedAsync(
            page,
            pageSize,
            keyword);

        if (result.TotalPages > 0 && page > result.TotalPages)
        {
            return RedirectToAction(nameof(Index), new
            {
                keyword,
                page = result.TotalPages,
                pageSize
            });
        }

        ViewBag.Keyword = keyword;

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadRolesAsync();

        return View(new CreateUserDto
        {
            IsActive = true
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadRolesAsync(dto.RoleId);
            return View(dto);
        }

        try
        {
            await _userService.AddAsync(dto);

            TempData["Success"] = "Kullanıcı başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            await LoadRolesAsync(dto.RoleId);

            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        var dto = new UpdateUserDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            RoleId = user.RoleId
        };

        await LoadRolesAsync(dto.RoleId);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadRolesAsync(dto.RoleId);
            return View(dto);
        }

        try
        {
            await _userService.UpdateAsync(dto);

            TempData["Success"] = "Kullanıcı başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            await LoadRolesAsync(dto.RoleId);

            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _userService.ToggleStatusAsync(id);

            TempData["Success"] = "Kullanıcı durumu güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _userService.DeleteAsync(id);

            TempData["Success"] = "Kullanıcı silindi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> ChangePassword(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        ViewBag.UserFullName = user.FullName;
        ViewBag.Username = user.Username;

        return View(new ChangePasswordDto
        {
            UserId = user.UserId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            var user = await _userService.GetByIdAsync(dto.UserId);

            if (user != null)
            {
                ViewBag.UserFullName = user.FullName;
                ViewBag.Username = user.Username;
            }

            return View(dto);
        }

        try
        {
            await _userService.ChangePasswordAsync(dto);

            TempData["Success"] = "Kullanıcı şifresi başarıyla değiştirildi.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            var user = await _userService.GetByIdAsync(dto.UserId);

            if (user != null)
            {
                ViewBag.UserFullName = user.FullName;
                ViewBag.Username = user.Username;
            }

            return View(dto);
        }
    }

    private async Task LoadRolesAsync(int? selectedRoleId = null)
    {
        var roles = await _roleService.GetAllAsync();

        ViewBag.Roles = new SelectList(
            roles,
            "RoleId",
            "RoleName",
            selectedRoleId);
    }
}