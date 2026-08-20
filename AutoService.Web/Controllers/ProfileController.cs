using Microsoft.AspNetCore.Mvc;
using AutoService.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AutoService.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdValue))
            {
                return RedirectToAction("Index", "Login");
            }

            if (!int.TryParse(userIdValue, out int userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var profile = await _profileService.GetProfileAsync(userId);

            if (profile == null)
            {
                TempData["Error"] = "Kullanıcı bilgileri bulunamadı.";

                return RedirectToAction("Index", "Dashboard");
            }

            return View(profile);
        }
    }
}     
    

