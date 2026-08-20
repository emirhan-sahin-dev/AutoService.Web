using AutoService.Business.Services.Abstract;
using AutoService.Dto.DashboardDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoService.Web.Models;
using System.Diagnostics;

namespace AutoService.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDashboardService _dashboardService;

    public HomeController(
        ILogger<HomeController> logger,
        IDashboardService dashboardService)
    {
        _logger = logger;
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        DashboardDto values = await _dashboardService.GetStatisticsAsync();

        return View(values);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}