using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.SparePartDtos;

public class CreateSparePartDto
{
    [Display(Name = "Parça Adı")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string PartName { get; set; } = null!;

    [Display(Name = "Parça Kodu")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(30, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string PartCode { get; set; } = null!;

    [Display(Name = "Birim Fiyat")]
    [Range(0.01, 9999999, ErrorMessage = "{0} 0'dan büyük olmalıdır.")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Stok Miktarı")]
    [Range(0, 999999, ErrorMessage = "{0} negatif olamaz.")]
    public int StockQuantity { get; set; }
}