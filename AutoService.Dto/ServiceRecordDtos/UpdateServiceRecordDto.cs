using AutoService.Entity.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceRecordDtos;

public class UpdateServiceRecordDto
{
    public ServiceStatus Status;
    public DateTime? ActualDeliveryDate;

    [Display(Name = "Giriş Tarihi")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public DateTime CheckInDate { get; set; } = DateTime.Now;

    [Display(Name = "Tahmini Teslim Tarihi")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public DateTime EstimatedDeliveryDate { get; set; }

    [Display(Name = "Kilometre")]
    [Range(
        0,
        9_999_999,
        ErrorMessage = "{0} negatif olamaz.")]
    public int Mileage { get; set; }

    [Display(Name = "Müşteri Şikâyeti")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(
        500,
        MinimumLength = 5,
        ErrorMessage = "{0} 5 ile 500 karakter arasında olmalıdır.")]
    public string CustomerComplaint { get; set; } = null!;

    [Display(Name = "Servis Kabul Açıklaması")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string? Description { get; set; }

    [Display(Name = "Araç")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Araç seçiniz.")]
    public int VehicleId { get; set; }
    public int ServiceRecordId { get; set; }

    [Display(Name = "Yakıt Seviyesi")]
    public FuelLevel FuelLevel { get; set; }

    [Display(Name = "Araç Üzerindeki Hasarlar")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla 1000 karakter olabilir.")]
    public string? ExistingDamages { get; set; }

    [Display(Name = "Teslim Edilen Eşyalar")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla 1000 karakter olabilir.")]
    public string? DeliveredItems { get; set; }

    [Display(Name = "Servis Danışmanı")]
    [StringLength(
        150,
        ErrorMessage = "{0} en fazla 150 karakter olabilir.")]
    public string? AdvisorName { get; set; }

    [Display(Name = "Müşteri Notu")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla 1000 karakter olabilir.")]
    public string? CustomerNotes { get; set; }
    [Display(Name = "Aracı Teslim Eden")]
    [StringLength(
    150,
    ErrorMessage = "{0} en fazla 150 karakter olabilir.")]
    public string? VehicleDeliveredBy { get; set; }

    [Display(Name = "Teslim Eden Telefon")]
    [StringLength(
        30,
        ErrorMessage = "{0} en fazla 30 karakter olabilir.")]
    public string? VehicleDeliveredByPhone { get; set; }

    [Display(Name = "Ön Onay Limiti")]
    [Range(
        0,
        10000000,
        ErrorMessage = "{0} geçerli bir tutar olmalıdır.")]
    public decimal PreApprovalLimit { get; set; }

    [Display(Name = "Ek İşlem Öncesi Onay Gerekli")]
    public bool RequiresApprovalForExtraWork { get; set; } = true;

    [Display(Name = "Eski Parçalar Müşteriye Verilsin")]
    public bool ReturnOldPartsToCustomer { get; set; }
}