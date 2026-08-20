using AutoService.Business.Abstract;
using AutoService.Dto.SystemSettingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemSettingController : Controller
    {
        private readonly ISystemSettingService _systemSettingService;

        public SystemSettingController(
            ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var setting = await _systemSettingService.GetAsync();

            if (setting == null)
            {
                TempData["Error"] = "Sistem ayarları kaydı bulunamadı.";

                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }

            var dto = new UpdateSystemSettingDto
            {
                SystemSettingId = setting.SystemSettingId,
                CompanyName = setting.CompanyName,
                CompanyPhone = setting.CompanyPhone,
                CompanyEmail = setting.CompanyEmail,
                CompanyAddress = setting.CompanyAddress,
                VatRate = setting.VatRate,
                CriticalStockLevel = setting.CriticalStockLevel,
                SessionTimeoutMinutes = setting.SessionTimeoutMinutes,
                Currency = setting.Currency
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            UpdateSystemSettingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _systemSettingService.UpdateAsync(dto);

                TempData["Success"] =
                    "Sistem ayarları başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}