using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceRecordDtos;

public class ResultServiceRecordDto
{
    public int ServiceRecordId { get; set; }

    public int VehicleId { get; set; }

    public string Plate { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public DateTime CheckInDate { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public DateTime? ActualDeliveryDate { get; set; }

    public int Mileage { get; set; }

    public string CustomerComplaint { get; set; } = null!;

    public string? Description { get; set; }

    public ServiceStatus Status { get; set; }

    public decimal TotalLaborPrice { get; set; }

    public decimal TotalPartsPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal TotalMechanicPayment { get; set; }

    public decimal TotalLaborGrossMargin { get; set; }

    public int? AppointmentId { get; set; }

    public bool IsCreatedFromAppointment =>
        AppointmentId.HasValue;
}