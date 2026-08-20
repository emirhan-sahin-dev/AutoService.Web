using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.DashboardDtos;

namespace AutoService.Business.Services.Concrete;

public class DashboardManager : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardManager(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardDto> GetStatisticsAsync()
    {
        return await _dashboardRepository.GetStatisticsAsync();
    }
}
