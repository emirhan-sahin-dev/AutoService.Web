using AutoService.Entity.Entities.Base;
using AutoService.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities
{
    public class ServiceOperation : BaseEntity
    {
        public int ServiceOperationId { get; set; }

        public int ServiceRecordId { get; set; }

        public int ServiceOperationTypeId { get; set; }

        public int MechanicId { get; set; }

        public string? ProblemDescription { get; set; }

        public string? WorkDescription { get; set; }

        public decimal LaborHours { get; set; }

        public decimal CustomerLaborPrice { get; set; }

        public decimal MechanicPayment { get; set; }

        public decimal LaborGrossMargin { get; set; }

        public ServiceOperationStatus Status { get; set; }
            = ServiceOperationStatus.Waiting;

        public DateTime? StartedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        // Navigation Properties

        public ServiceRecord ServiceRecord { get; set; } = null!;

        public ServiceOperationType ServiceOperationType { get; set; } = null!;

        public Mechanic Mechanic { get; set; } = null!;

        public ICollection<ServiceOperationPart> ServiceOperationParts { get; set; }
            = new List<ServiceOperationPart>();
    }
}