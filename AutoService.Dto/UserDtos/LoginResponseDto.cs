using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.UserDtos;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
}
