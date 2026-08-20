using AutoService.Dto.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;

namespace AutoService.Dto.ServiceRecordDtos;

public class ServiceExitReceiptDto
{
    public int ServiceRecordId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public DateTime? ActualDeliveryDate { get; set; }

    public int Mileage { get; set; }

    public ServiceStatus Status { get; set; }

    public string? Description { get; set; }

    public string? AdvisorName { get; set; }

    // Müşteri
    public string CustomerName { get; set; } = "-";

    public string CustomerPhone { get; set; } = "-";

    public string CustomerEmail { get; set; } = "-";

    public string CustomerAddress { get; set; } = "-";

    // Araç
    public string Plate { get; set; } = "-";

    public string VinNumber { get; set; } = "-";

    public int ModelYear { get; set; }

    public string BrandName { get; set; } = "-";

    public string ModelName { get; set; } = "-";

    // Firma
    public string CompanyName { get; set; } = "Auto Service";

    public string? CompanyPhone { get; set; }

    public string? CompanyEmail { get; set; }

    public string? CompanyAddress { get; set; }

    public decimal VatRate { get; set; }

    public string Currency { get; set; } = "TRY";

    // Kalemler
    public List<ServiceExitOperationItemDto> Operations { get; set; }
        = new();

    public List<ServiceExitPartItemDto> Parts { get; set; }
        = new();

    // Toplamlar
    public decimal LaborSubtotal { get; set; }

    public decimal PartsSubtotal { get; set; }

    public decimal Subtotal { get; set; }

    public decimal VatAmount { get; set; }

    public decimal GrandTotal { get; set; }

    public int TotalOperationCount { get; set; }

    public int TotalPartQuantity { get; set; }
    public decimal TotalPaid { get; set; }

    public decimal RemainingAmount { get; set; }

    public string PaymentStatusText { get; set; } = "Ödeme Bekleniyor";

    public bool IsFullyPaid { get; set; }

    public bool HasPartialPayment { get; set; }

    public List<ResultPaymentDto> Payments { get; set; }
        = new();
}
