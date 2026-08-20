using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class DashboardDto
{
    public int TotalCustomers { get; set; }

    public int TotalVehicles { get; set; }

    public int TotalMechanics { get; set; }

    public int TotalServiceRecords { get; set; }

    public int TotalSpareParts { get; set; }

    public int LowStockSpareParts { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public int TodayServiceCount { get; set; }

    public int ActiveServiceCount { get; set; }

    public decimal AverageServicePrice { get; set; }
    public decimal TotalRevenue { get; set; }
    public int WaitingDelivery { get; set; }

    public int ActiveServices { get; set; }

    public int CriticalStockCount { get; set; }
    public List<RecentServiceDto> RecentServices { get; set; } = new();

    public List<CriticalStockDto> CriticalStocks { get; set; } = new();

    public List<UpcomingDeliveryDto> UpcomingDeliveries { get; set; } = new();

    public List<DashboardChartDto> MonthlyRevenueChart { get; set; } = new();
    public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
    public int NewCustomersThisMonth { get; set; }

    public decimal RevenueLastMonth { get; set; }

    public decimal RevenueThisMonth { get; set; }

    public int CompletedServicesThisMonth { get; set; }

    public string MostPopularBrand { get; set; } = "-";

    public int MostPopularBrandServiceCount { get; set; }

    public string MostActiveMechanic { get; set; } = "-";

    public int MostActiveMechanicServiceCount { get; set; }

    public decimal RevenueChangePercentage { get; set; }
    public bool RevenueIncreased => RevenueThisMonth >= RevenueLastMonth;

    public List<BrandDistributionDto> BrandDistributions { get; set; } = new();

    public List<ServiceStatusChartDto> ServiceStatusCharts { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}