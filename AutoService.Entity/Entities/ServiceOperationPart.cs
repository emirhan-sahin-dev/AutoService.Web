using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class ServiceOperationPart : BaseEntity
{
    public int ServiceOperationPartId { get; set; }

    public int ServiceOperationId { get; set; }

    public ServiceOperation ServiceOperation { get; set; } = null!;

    public int SparePartId { get; set; }

    public SparePart SparePart { get; set; } = null!;

    public int Quantity { get; set; }

    /*
     * Parçanın işlem yapıldığı andaki satış fiyatı.
     * Sonradan SparePart.UnitPrice değişse bile
     * eski servis tutarı değişmez.
     */
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
