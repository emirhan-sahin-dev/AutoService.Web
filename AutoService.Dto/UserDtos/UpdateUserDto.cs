using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.UserDtos;

public class UpdateUserDto
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Rol seçimi zorunludur.")]
    public int RoleId { get; set; }
}