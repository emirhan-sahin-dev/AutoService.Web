using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.VehicleDtos;

public class UpdateVehicleDto
{
    public int VehicleId { get; set; }

    [Display(Name = "Plaka")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(15, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string Plate { get; set; } = null!;

    [Display(Name = "Şasi Numarası (VIN)")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string VinNumber { get; set; } = null!;

    [Display(Name = "Model Yılı")]
    [Range(1950, 2100, ErrorMessage = "Geçerli bir model yılı giriniz.")]
    public int ModelYear { get; set; }

    [Display(Name = "Kilometre")]
    [Range(0, 9999999, ErrorMessage = "{0} negatif olamaz.")]
    public int Mileage { get; set; }

    [Display(Name = "Marka")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public int BrandId { get; set; }

    [Display(Name = "Model")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public int ModelId { get; set; }

    [Display(Name = "Müşteri")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public int CustomerId { get; set; }
}