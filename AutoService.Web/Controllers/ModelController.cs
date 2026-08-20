using AutoService.Business.Services.Abstract;
using AutoService.Dto.ModelDtos;
using AutoService.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Web.Controllers
{
    [Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
    public class ModelController : Controller
    {
        private readonly IModelService _modelService;
        private readonly IBrandService _brandService;

        public ModelController(
            IModelService modelService,
            IBrandService brandService)
        {
            _modelService = modelService;
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await _modelService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();

            ViewBag.Brands = brands.ToDictionary(
                x => x.BrandId,
                x => x.BrandName);

            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = await _modelService.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var brand = await _brandService.GetByIdAsync(model.BrandId);

            ViewBag.BrandName = brand?.BrandName ?? "Marka bulunamadı";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadBrandsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadBrandsAsync(dto.BrandId);

                return View(dto);
            }

            await _modelService.AddAsync(dto);

            TempData["Message"] = "Model başarıyla eklendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _modelService.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            var dto = new UpdateModelDto
            {
                ModelId = model.ModelId,
                ModelName = model.ModelName,
                BrandId = model.BrandId
            };

            await LoadBrandsAsync(dto.BrandId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateModelDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadBrandsAsync(dto.BrandId);

                return View(dto);
            }

            await _modelService.UpdateAsync(dto);

            TempData["Message"] = "Model başarıyla güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _modelService.DeleteAsync(id);

            TempData["Message"] = "Model başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetByBrand(int brandId)
        {
            var models =
                await _modelService.GetModelsByBrandIdAsync(brandId);

            return Json(models);
        }

        private async Task LoadBrandsAsync(int? selectedBrandId = null)
        {
            var brands = await _brandService.GetAllAsync();

            ViewBag.Brands = new SelectList(
                brands,
                "BrandId",
                "BrandName",
                selectedBrandId);
        }
    }
}