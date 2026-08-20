using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.AccountSettingDtos;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = "Yeni şifre tekrarı zorunludur.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Yeni şifreler uyuşmuyor.")]
    public string ConfirmPassword { get; set; } = null!;
}