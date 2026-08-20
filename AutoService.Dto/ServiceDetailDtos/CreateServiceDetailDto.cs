using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceDetailDtos;

public class CreateServiceDetailDto
{
    [Display(Name = "Servis Kaydı")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public int ServiceRecordId { get; set; }

    [Display(Name = "Yedek Parça")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public int SparePartId { get; set; }

    [Display(Name = "Adet")]
    [Range(1, 9999, ErrorMessage = "{0} en az 1 olmalıdır.")]
    public int Quantity { get; set; }

    [Display(Name = "Birim Fiyat")]
    [Range(0.01, 99999999, ErrorMessage = "{0} 0'dan büyük olmalıdır.")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Toplam Tutar")]
    [Range(0.01, 99999999, ErrorMessage = "{0} 0'dan büyük olmalıdır.")]
    public decimal TotalPrice { get; set; }
}