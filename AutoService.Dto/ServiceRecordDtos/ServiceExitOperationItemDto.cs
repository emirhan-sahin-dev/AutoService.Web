using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceRecordDtos;

public class ServiceExitOperationItemDto
{
    public int ServiceOperationId { get; set; }

    public string OperationTypeName { get; set; } = "-";

    public string MechanicFullName { get; set; } = "-";

    public string SpecialtyName { get; set; } = "-";

    public string? ProblemDescription { get; set; }

    public string? WorkDescription { get; set; }

    public decimal LaborHours { get; set; }

    public decimal CustomerLaborPrice { get; set; }

    public ServiceOperationStatus Status { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }
}