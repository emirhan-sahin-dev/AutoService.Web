using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.RoleDtos;

public class GetByIdRoleDto
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int UserCount { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
}
