using AutoService.Entity.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceRecordDtos;

public class ServiceRecordDetailDto
{
    public int ServiceRecordId { get; set; }

    [Display(Name = "Araç")]
    public int VehicleId { get; set; }

    [Display(Name = "Araç Plakası")]
    public string Plate { get; set; } = null!;

    [Display(Name = "Müşteri")]
    public string CustomerName { get; set; } = null!;

    [Display(Name = "Giriş Tarihi")]
    public DateTime CheckInDate { get; set; }

    [Display(Name = "Tahmini Teslim Tarihi")]
    public DateTime? EstimatedDeliveryDate { get; set; }

    [Display(Name = "Gerçek Teslim Tarihi")]
    public DateTime? ActualDeliveryDate { get; set; }

    [Display(Name = "Kilometre")]
    public int Mileage { get; set; }

    [Display(Name = "Servis Durumu")]
    public ServiceStatus Status { get; set; }

    [Display(Name = "Müşteri Şikayeti")]
    public string CustomerComplaint { get; set; } = null!;

    [Display(Name = "Servis Kabul Açıklaması")]
    public string? Description { get; set; }
    [Display(Name = "Yakıt Seviyesi")]
    public FuelLevel FuelLevel { get; set; }

    [Display(Name = "Araç Üzerindeki Hasarlar")]
    public string? ExistingDamages { get; set; }

    [Display(Name = "Teslim Edilen Eşyalar")]
    public string? DeliveredItems { get; set; }

    [Display(Name = "Servis Danışmanı")]
    public string? AdvisorName { get; set; }

    [Display(Name = "Müşteri Notu")]
    public string? CustomerNotes { get; set; }

    // ---------- Finansal Özet ----------

    [Display(Name = "Toplam İşçilik")]
    public decimal TotalLaborPrice { get; set; }

    [Display(Name = "Toplam Parça")]
    public decimal TotalPartsPrice { get; set; }

    [Display(Name = "Genel Toplam")]
    public decimal TotalPrice { get; set; }

    [Display(Name = "Toplam Usta Hakedişi")]
    public decimal TotalMechanicPayment { get; set; }

    [Display(Name = "Brüt İşçilik Kazancı")]
    public decimal TotalLaborGrossMargin { get; set; }

    // ---------- İstatistik ----------

    public int TotalOperationCount { get; set; }

    public int TotalPartCount { get; set; }

    public string? VehicleDeliveredBy { get; set; }

    public string? VehicleDeliveredByPhone { get; set; }

    public decimal PreApprovalLimit { get; set; }

    public bool RequiresApprovalForExtraWork { get; set; }

    public bool ReturnOldPartsToCustomer { get; set; }

    public int? AppointmentId { get; set; }

    public bool IsCreatedFromAppointment =>
        AppointmentId.HasValue;
}