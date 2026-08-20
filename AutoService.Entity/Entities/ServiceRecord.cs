using AutoService.Entity.Entities.Base;
using AutoService.Entity.Enums;

namespace AutoService.Entity.Entities;

public class ServiceRecord : BaseEntity
{
    public int ServiceRecordId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public DateTime? ActualDeliveryDate { get; set; }

    public int Mileage { get; set; }

    // Eski kolonlar şimdilik veritabanında kalıyorsa 0 tutulabilir.
    public decimal LaborCost { get; set; }

    public decimal TotalPrice { get; set; }

    public string CustomerComplaint { get; set; } = null!;

    public string? Description { get; set; }

    public ServiceStatus Status { get; set; }

    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = null!;

    public ICollection<ServiceOperation> ServiceOperations { get; set; }
        = new List<ServiceOperation>();

    // Araç teslim alınırken işaretlenecek bilgiler

    public FuelLevel FuelLevel { get; set; }

    public string? ExistingDamages { get; set; }

    public string? DeliveredItems { get; set; }

    public string? AdvisorName { get; set; }

    public string? CustomerNotes { get; set; }
    public string? VehicleDeliveredBy { get; set; }

    public string? VehicleDeliveredByPhone { get; set; }

    public decimal PreApprovalLimit { get; set; }

    public bool RequiresApprovalForExtraWork { get; set; } = true;

    public bool ReturnOldPartsToCustomer { get; set; }

    public ICollection<Payment> Payments { get; set; }
    = new List<Payment>();

    public Appointment? Appointment { get; set; }
}