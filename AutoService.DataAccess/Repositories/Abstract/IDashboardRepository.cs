using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.DashboardDtos;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IDashboardRepository
{
    Task<DashboardDto> GetStatisticsAsync();
}
