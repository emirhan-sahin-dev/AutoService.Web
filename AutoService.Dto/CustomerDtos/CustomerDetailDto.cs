using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.CustomerDtos;

public class CustomerDetailDto
{
    public int CustomerId { get; set; }

    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Telefon")]
    public string Phone { get; set; } = null!;

    [Display(Name = "E-Posta")]
    public string Email { get; set; } = null!;

    [Display(Name = "Adres")]
    public string Address { get; set; } = null!;

    [Display(Name = "Araç Sayısı")]
    public int VehicleCount { get; set; }
}