using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ReportDtos;

public class LowStockPartDto
{
    public int SparePartId { get; set; }

    public string PartName { get; set; } = string.Empty;

    public string PartCode { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string StockStatus { get; set; } = string.Empty;
}
