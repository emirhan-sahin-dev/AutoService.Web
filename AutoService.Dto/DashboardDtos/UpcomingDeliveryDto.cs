using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class UpcomingDeliveryDto
{
    public string Plate { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public DateTime DeliveryDate { get; set; }
}