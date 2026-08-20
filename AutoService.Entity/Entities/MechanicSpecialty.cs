using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class MechanicSpecialty : BaseEntity
{
    public int MechanicSpecialtyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Mechanic> Mechanics { get; set; }
        = new List<Mechanic>();

    public ICollection<ServiceOperationType> ServiceOperationTypes { get; set; }
        = new List<ServiceOperationType>();
}
