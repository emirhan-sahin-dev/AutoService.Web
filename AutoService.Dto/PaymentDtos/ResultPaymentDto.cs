using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.PaymentDtos;

public class ResultPaymentDto
{
    public int PaymentId { get; set; }

    public int ServiceRecordId { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? Description { get; set; }

    public string? TransactionReference { get; set; }
}