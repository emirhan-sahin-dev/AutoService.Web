using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceRecordDtos;

public class ServiceAcceptanceFormDto
{
    // Servis bilgileri
    public int ServiceRecordId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public int Mileage { get; set; }

    public ServiceStatus Status { get; set; }

    public string CustomerComplaint { get; set; } = null!;

    public string? Description { get; set; }


    // Müşteri bilgileri
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;


    // Araç bilgileri
    public int VehicleId { get; set; }

    public string Plate { get; set; } = null!;

    public string VinNumber { get; set; } = null!;

    public int ModelYear { get; set; }

    public string BrandName { get; set; } = null!;

    public string ModelName { get; set; } = null!;


    // Kabul sırasında girilen bilgiler
    public FuelLevel FuelLevel { get; set; }

    public string? ExistingDamages { get; set; }

    public string? DeliveredItems { get; set; }

    public string? AdvisorName { get; set; }

    public string? CustomerNotes { get; set; }


    // Firma bilgileri
    public string CompanyName { get; set; } = "Auto Service";

    public string? CompanyPhone { get; set; }

    public string? CompanyEmail { get; set; }

    public string? CompanyAddress { get; set; }

    public string? VehicleDeliveredBy { get; set; }

    public string? VehicleDeliveredByPhone { get; set; }

    public decimal PreApprovalLimit { get; set; }

    public bool RequiresApprovalForExtraWork { get; set; }

    public bool ReturnOldPartsToCustomer { get; set; }
}
