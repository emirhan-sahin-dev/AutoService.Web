using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities.Base;
using AutoService.Entity.Enums;

namespace AutoService.Entity.Entities;

public class Payment : BaseEntity
{
    public int PaymentId { get; set; }

    public int ServiceRecordId { get; set; }

    public ServiceRecord ServiceRecord { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    public string? TransactionReference { get; set; }
}
