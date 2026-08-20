using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.DashboardDtos;

public class BrandDistributionDto
{
    public string BrandName { get; set; } = string.Empty;

    public int VehicleCount { get; set; }
}