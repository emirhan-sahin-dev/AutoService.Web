using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;
using AutoService.Entity.Enums;

namespace AutoService.Entity.Entities;

public class Appointment : BaseEntity
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }

    public string CustomerRequest { get; set; } = null!;

    public string? Description { get; set; }

    public AppointmentStatus Status { get; set; }
        = AppointmentStatus.Waiting;

    /*
     * Randevudan servis kaydı oluşturulduysa
     * hangi servis kaydına dönüştüğünü tutar.
     */
    public int? ServiceRecordId { get; set; }

    public ServiceRecord? ServiceRecord { get; set; }
}
