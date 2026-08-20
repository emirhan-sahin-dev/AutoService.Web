using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.UserDtos;

public class ChangePasswordDto
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Şifreler birbiriyle uyuşmuyor.")]
    public string ConfirmPassword { get; set; } = null!;
}
