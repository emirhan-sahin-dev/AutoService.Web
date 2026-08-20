using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceOperationDtos;

public class ResultServiceOperationDto
{
    public int ServiceOperationId { get; set; }

    public int ServiceRecordId { get; set; }

    public string VehiclePlate { get; set; } = null!;

    public string OperationTypeName { get; set; } = null!;

    public string SpecialtyName { get; set; } = null!;

    public int MechanicId { get; set; }

    public string MechanicFullName { get; set; } = null!;

    public decimal LaborHours { get; set; }

    public decimal CustomerLaborPrice { get; set; }

    public decimal MechanicPayment { get; set; }

    public decimal LaborGrossMargin { get; set; }

    public decimal PartsTotal { get; set; }

    public decimal CustomerTotal { get; set; }

    public ServiceOperationStatus Status { get; set; }

    public DateTime CreatedDate { get; set; }
}
