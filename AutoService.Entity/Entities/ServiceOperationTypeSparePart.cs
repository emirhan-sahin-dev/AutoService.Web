using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class ServiceOperationTypeSparePart : BaseEntity
{
    public int ServiceOperationTypeSparePartId { get; set; }

    public int ServiceOperationTypeId { get; set; }

    public ServiceOperationType ServiceOperationType { get; set; }
        = null!;

    public int SparePartId { get; set; }

    public SparePart SparePart { get; set; }
        = null!;
}
