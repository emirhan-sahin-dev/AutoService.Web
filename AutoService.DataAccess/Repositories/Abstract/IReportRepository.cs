using AutoService.Dto.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IReportRepository
{
    Task<ReportDashboardDto> GetDashboardReportAsync(
        DateTime? startDate,
        DateTime? endDate);
}
