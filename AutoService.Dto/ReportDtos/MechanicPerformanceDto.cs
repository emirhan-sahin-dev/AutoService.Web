using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ReportDtos;

public class MechanicPerformanceDto
{
    public int MechanicId { get; set; }

    public string MechanicName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    public int TotalServiceCount { get; set; }

    public int DeliveredServiceCount { get; set; }

    public int ActiveServiceCount { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal TotalLaborCost { get; set; }
}
