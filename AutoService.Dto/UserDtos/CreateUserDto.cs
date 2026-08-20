using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.UserDtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Rol seçimi zorunludur.")]
    public int RoleId { get; set; }
}

