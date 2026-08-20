using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ServiceRecordDtos;

public class ServiceExitPartItemDto
{
    public int ServiceOperationPartId { get; set; }

    public int ServiceOperationId { get; set; }

    public string OperationTypeName { get; set; } = "-";

    public int SparePartId { get; set; }

    public string PartName { get; set; } = "-";

    public string PartCode { get; set; } = "-";

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
