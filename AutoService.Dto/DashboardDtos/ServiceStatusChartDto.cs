using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class ServiceStatusChartDto
{
    public string StatusName { get; set; } = string.Empty;

    public int Count { get; set; }
}
