using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class ServiceOperationType : BaseEntity
{
    public int ServiceOperationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    /*
     * İşlemin standart tamamlanma süresi.
     * Örnek: Fren balatası değişimi = 1.5 saat
     */
    public decimal DefaultDurationHours { get; set; }

    /*
     * Müşteriye yansıtılan varsayılan işçilik bedeli.
     * Örnek: 1.200 TL
     */
    public decimal CustomerLaborPrice { get; set; }

    /*
     * İşlemi tamamlayan ustaya yazılan hakediş.
     * Örnek: 300 TL
     */
    public decimal MechanicPayment { get; set; }

    public bool IsActive { get; set; } = true;

    /*
     * Bu işlemi hangi uzmanlık alanındaki ustaların
     * yapabileceğini belirler.
     */
    public int MechanicSpecialtyId { get; set; }

    public MechanicSpecialty MechanicSpecialty { get; set; } = null!;

    public ICollection<ServiceOperation> ServiceOperations { get; set; }
        = new List<ServiceOperation>();

    public ICollection<ServiceOperationTypeSparePart>
    ServiceOperationTypeSpareParts
    { get; set; }
        = new List<ServiceOperationTypeSparePart>();
}
