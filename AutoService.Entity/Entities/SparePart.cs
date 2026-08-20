using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class SparePart : BaseEntity
{
    public int SparePartId { get; set; }

    public string PartName { get; set; } = null!;

    public string PartCode { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    /*
     * Eski ilişki.
     * Yeni sistem tamamlandıktan sonra kaldırılacak.
     */
    public ICollection<ServiceDetail> ServiceDetails { get; set; }
        = new List<ServiceDetail>();

    /*
     * Yeni işlem-parça ilişkisi.
     */
    public ICollection<ServiceOperationPart> ServiceOperationParts { get; set; }
        = new List<ServiceOperationPart>();

    public ICollection<ServiceOperationTypeSparePart>
    ServiceOperationTypeSpareParts
    { get; set; }
        = new List<ServiceOperationTypeSparePart>();
}