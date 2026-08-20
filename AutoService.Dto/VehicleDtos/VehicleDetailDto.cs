using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.VehicleDtos;

public class VehicleDetailDto
{
    public int VehicleId { get; set; }

    [Display(Name = "Plaka")]
    public string Plate { get; set; } = null!;

    [Display(Name = "Şasi Numarası (VIN)")]
    public string VinNumber { get; set; } = null!;

    [Display(Name = "Model Yılı")]
    public int ModelYear { get; set; }

    [Display(Name = "Kilometre")]
    public int Mileage { get; set; }

    [Display(Name = "Marka")]
    public string BrandName { get; set; } = null!;

    [Display(Name = "Model")]
    public string ModelName { get; set; } = null!;

    [Display(Name = "Müşteri")]
    public string CustomerName { get; set; } = null!;
}