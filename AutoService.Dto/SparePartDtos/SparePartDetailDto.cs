using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.SparePartDtos;

public class SparePartDetailDto
{
    public int SparePartId { get; set; }

    [Display(Name = "Parça Adı")]
    public string PartName { get; set; } = null!;

    [Display(Name = "Parça Kodu")]
    public string PartCode { get; set; } = null!;

    [Display(Name = "Birim Fiyat")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
}