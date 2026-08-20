using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class ServiceDetail : BaseEntity
{
    public int ServiceDetailId { get; set; }

    public int ServiceRecordId { get; set; }

    public ServiceRecord ServiceRecord { get; set; } = null!;

    public int SparePartId { get; set; }

    public SparePart SparePart { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}