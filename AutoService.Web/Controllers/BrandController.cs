using AutoService.Business.Services.Abstract;
using AutoService.Dto.BrandDtos;
using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers
{
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _brandService.GetAllAsync();

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var value = await _brandService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _brandService.AddAsync(dto);

            TempData["Message"] = "Marka başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var value = await _brandService.GetByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var dto = new UpdateBrandDto
            {
                BrandId = value.BrandId,
                BrandName = value.BrandName
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _brandService.UpdateAsync(dto);

            TempData["Message"] = "Marka başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteAsync(id);

            TempData["Message"] = "Marka başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}