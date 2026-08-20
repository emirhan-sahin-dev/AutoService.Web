using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class CriticalStockDto
{
    public string PartName { get; set; } = null!;

    public int StockQuantity { get; set; }
}
