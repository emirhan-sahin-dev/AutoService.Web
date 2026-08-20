using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ReportDtos;

public class MonthlyRevenueDto
{
    public int Year { get; set; }

    public int Month { get; set; }

    public string MonthName { get; set; } = string.Empty;

    public int ServiceCount { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal LaborRevenue { get; set; }

    public decimal PartRevenue { get; set; }
}