using AutoService.Dto.ReportDtos;

namespace AutoService.Web.Services;

public interface IReportExportService
{
    byte[] CreateExcel(ReportDashboardDto report);
    byte[] CreatePdf(ReportDashboardDto report);
}
