using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.AppointmentDtos;

public class AppointmentDetailDto
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = "-";

    public string CustomerPhone { get; set; } = "-";

    public string CustomerEmail { get; set; } = "-";

    public int VehicleId { get; set; }

    public string Plate { get; set; } = "-";

    public string VinNumber { get; set; } = "-";

    public string BrandName { get; set; } = "-";

    public string ModelName { get; set; } = "-";

    public int ModelYear { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string CustomerRequest { get; set; } = "-";

    public string? Description { get; set; }

    public AppointmentStatus Status { get; set; }

    public int? ServiceRecordId { get; set; }

    public DateTime CreatedDate { get; set; }
}
