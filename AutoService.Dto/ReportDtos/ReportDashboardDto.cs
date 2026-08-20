using AutoService.Dto.DashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ReportDtos;

public class ReportDashboardDto
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int TotalServiceCount { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal TotalLaborCost { get; set; }

    public int DeliveredVehicleCount { get; set; }

    public int ActiveServiceCount { get; set; }

    public int TotalUsedPartQuantity { get; set; }

    public int LowStockPartCount { get; set; }

    public List<MonthlyRevenueDto> MonthlyRevenues { get; set; }
        = new List<MonthlyRevenueDto>();

    public List<MechanicPerformanceDto> MechanicPerformances { get; set; }
        = new List<MechanicPerformanceDto>();

    public List<MostUsedPartDto> MostUsedParts { get; set; }
        = new List<MostUsedPartDto>();

    public List<LowStockPartDto> LowStockParts { get; set; }
        = new List<LowStockPartDto>();
}