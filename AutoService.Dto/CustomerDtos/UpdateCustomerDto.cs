using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.CustomerDtos;

public class UpdateCustomerDto
{
    public int CustomerId { get; set; }

    [Display(Name = "Ad Soyad")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Telefon")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Phone { get; set; } = null!;

    [Display(Name = "E-Posta")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Adres")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(250, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string Address { get; set; } = null!;
}