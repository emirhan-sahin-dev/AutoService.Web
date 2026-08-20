using AutoService.Business.Security;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.AccountSettingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoService.WebUI.Controllers;

[Authorize]
public class AccountSettingController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountSettingController(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
        {
            TempData["Error"] = "Oturum bilgisi bulunamadı.";
            return RedirectToAction("Index", "Login");
        }

        var user = await _userRepository.GetByIdWithRoleAsync(userId);

        if (user == null)
        {
            TempData["Error"] = "Kullanıcı bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        var currentPasswordCorrect =
            PasswordHasher.Verify(dto.CurrentPassword, user.PasswordHash);

        if (!currentPasswordCorrect)
        {
            ModelState.AddModelError(
                nameof(dto.CurrentPassword),
                "Mevcut şifreniz yanlış.");

            return View(dto);
        }

        var newPasswordSameAsCurrent =
            PasswordHasher.Verify(dto.NewPassword, user.PasswordHash);

        if (newPasswordSameAsCurrent)
        {
            ModelState.AddModelError(
                nameof(dto.NewPassword),
                "Yeni şifre mevcut şifrenizle aynı olamaz.");

            return View(dto);
        }

        user.PasswordHash = PasswordHasher.Hash(dto.NewPassword);
        user.UpdatedDate = DateTime.Now;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        TempData["Success"] = "Şifreniz başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }
}