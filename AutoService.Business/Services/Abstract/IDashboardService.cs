using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.DashboardDtos;

namespace AutoService.Business.Services.Abstract;

public interface IDashboardService
{
    Task<DashboardDto> GetStatisticsAsync();
}