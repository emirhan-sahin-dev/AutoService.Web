using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.UserDtos;

public class ResultUserDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}