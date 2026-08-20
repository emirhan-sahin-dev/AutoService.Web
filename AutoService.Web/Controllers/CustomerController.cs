using AutoService.Web.Security;
using AutoService.Business.Services.Abstract;
using AutoService.Dto.CustomerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Web.Controllers;

[Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<IActionResult> Index(int page = 1, string? keyword = null)
    {
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var searchResult = await _customerService.SearchAsync(keyword);

            ViewBag.Keyword = keyword;

            return View(searchResult);
        }

        var values = await _customerService.GetPagedAsync(page, 10);
        int totalCount = await _customerService.GetCountAsync();
        int pageSize = 10;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPage = (int)Math.Ceiling((double)totalCount / pageSize);

        ViewBag.Page = page;

        return View(values);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _customerService.AddAsync(dto);
        TempData["Success"] = "Müşteri Başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        var updatedto = new UpdateCustomerDto
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address
        };
        return View(updatedto);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);
        await _customerService.UpdateAsync(dto);
        TempData["Success"] = "Müşteri Güncellendi.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);
        TempData["Success"] = "Müşteri silindi.";
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            return NotFound();

        return View(customer);
    }
}
