using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;

    public decimal Revenue { get; set; }
}
