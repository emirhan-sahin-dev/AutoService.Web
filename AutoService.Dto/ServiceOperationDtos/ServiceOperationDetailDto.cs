using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceOperationDtos;

public class ServiceOperationDetailDto
{
    public int ServiceOperationId { get; set; }

    public int ServiceRecordId { get; set; }

    public string VehiclePlate { get; set; } = null!;

    public string OperationTypeName { get; set; } = null!;

    public string SpecialtyName { get; set; } = null!;

    public int MechanicId { get; set; }

    public string MechanicFullName { get; set; } = null!;

    public string? ProblemDescription { get; set; }

    public string? WorkDescription { get; set; }

    public decimal LaborHours { get; set; }

    public decimal CustomerLaborPrice { get; set; }

    public decimal MechanicPayment { get; set; }

    public decimal LaborGrossMargin { get; set; }

    public decimal PartsTotal { get; set; }

    public decimal CustomerTotal { get; set; }

    public ServiceOperationStatus Status { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public List<ServiceOperationPartItemDto> Parts { get; set; }
        = new();
}
