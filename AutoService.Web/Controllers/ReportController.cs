using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Web.Security;
using AutoService.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.WebUI.Controllers;

[Authorize(Roles = AppRoles.AdminOrServiceAdvisor)]
public class ReportController : Controller
{
    private readonly IReportRepository _reportRepository;
    private readonly IReportExportService _reportExportService;

    public ReportController(IReportRepository reportRepository, IReportExportService reportExportService)
    {
        _reportRepository = reportRepository;
        _reportExportService = reportExportService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
    {
        NormalizeDates(ref startDate, ref endDate);
        var report = await _reportRepository.GetDashboardReportAsync(startDate, endDate);
        return View(report);
    }

    [HttpGet]
    public async Task<IActionResult> ExportExcel(DateTime? startDate, DateTime? endDate)
    {
        NormalizeDates(ref startDate, ref endDate);
        var report = await _reportRepository.GetDashboardReportAsync(startDate, endDate);
        var file = _reportExportService.CreateExcel(report);
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"AutoService_Rapor_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> ExportPdf(DateTime? startDate, DateTime? endDate)
    {
        NormalizeDates(ref startDate, ref endDate);
        var report = await _reportRepository.GetDashboardReportAsync(startDate, endDate);
        var file = _reportExportService.CreatePdf(report);
        return File(file, "application/pdf", $"AutoService_Rapor_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
    }

    private void NormalizeDates(ref DateTime? startDate, ref DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue || startDate.Value.Date <= endDate.Value.Date) return;
        TempData["ErrorMessage"] = "Başlangıç tarihi, bitiş tarihinden büyük olamaz.";
        startDate = null;
        endDate = null;
    }
}
