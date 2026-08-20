using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class RecentServiceDto
{
    public int ServiceRecordId { get; set; }

    public string Plate { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public DateTime CheckInDate { get; set; }
    public string Status { get; set; } = string.Empty;


    public decimal TotalPrice { get; set; }
}