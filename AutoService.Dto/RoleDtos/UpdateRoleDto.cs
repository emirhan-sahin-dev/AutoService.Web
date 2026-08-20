using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.RoleDtos;

public class UpdateRoleDto
{
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Rol adı boş bırakılamaz.")]
    [StringLength(
        50,
        MinimumLength = 2,
        ErrorMessage = "Rol adı 2 ile 50 karakter arasında olmalıdır.")]
    public string RoleName { get; set; } = null!;
}