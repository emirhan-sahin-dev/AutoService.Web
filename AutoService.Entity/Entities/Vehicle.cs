using AutoService.Entity.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities
{
    public class Vehicle : BaseEntity
    {
        public int VehicleId { get; set; }

        public string Plate { get; set; } = null!;
        public string VinNumber { get; set; } = null!;

        public int ModelYear { get; set; }
        public int Mileage { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int ModelId { get; set; }
        public Model Model { get; set; } = null!;

        public ICollection<ServiceRecord> ServiceRecords { get; set; } = new List<ServiceRecord>();

        public ICollection<Appointment> Appointments { get; set; }
    = new List<Appointment>();
    }
}
