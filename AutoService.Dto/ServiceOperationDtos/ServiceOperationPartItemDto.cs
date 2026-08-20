using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ServiceOperationDtos;

public class ServiceOperationPartItemDto
{
    public int ServiceOperationPartId { get; set; }

    public int SparePartId { get; set; }

    public string PartName { get; set; } = null!;

    public string PartCode { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
