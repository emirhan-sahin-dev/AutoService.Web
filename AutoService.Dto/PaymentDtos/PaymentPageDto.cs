using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.PaymentDtos;

public class PaymentPageDto
{
    public int ServiceRecordId { get; set; }

    public string Plate { get; set; } = "-";

    public string CustomerName { get; set; } = "-";

    public decimal ServiceTotal { get; set; }

    public decimal TotalPaid { get; set; }

    public decimal RemainingAmount { get; set; }

    public bool IsFullyPaid =>
        RemainingAmount <= 0;

    public CreatePaymentDto NewPayment { get; set; }
        = new();

    public List<ResultPaymentDto> Payments { get; set; }
        = new();
}
