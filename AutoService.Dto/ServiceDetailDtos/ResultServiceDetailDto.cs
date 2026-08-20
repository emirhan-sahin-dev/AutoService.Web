using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceDetailDtos;

public class ResultServiceDetailDto
{
    public int ServiceDetailId { get; set; }

    [Display(Name = "Araç Plakası")]
    public string Plate { get; set; } = null!;

    [Display(Name = "Yedek Parça")]
    public string SparePartName { get; set; } = null!;

    [Display(Name = "Adet")]
    public int Quantity { get; set; }

    [Display(Name = "Birim Fiyat")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Toplam Tutar")]
    public decimal TotalPrice { get; set; }
}