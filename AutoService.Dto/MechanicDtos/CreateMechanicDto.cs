using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.MechanicDtos;

public class CreateMechanicDto
{
    [Display(Name = "Ad")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Soyad")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(50, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Telefon")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Phone { get; set; } = null!;

    [Display(Name = "E-Posta")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Uzmanlık Alanı")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public string Specialty { get; set; } = null!;

    [Display(Name = "İşe Giriş Tarihi")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    public DateTime HireDate { get; set; }
}