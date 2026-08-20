using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ReportDtos;

public class MostUsedPartDto
{
    public int SparePartId { get; set; }

    public string PartName { get; set; } = string.Empty;

    public string PartCode { get; set; } = string.Empty;

    public int TotalQuantity { get; set; }

    public int UsageCount { get; set; }

    public decimal TotalRevenue { get; set; }
}
