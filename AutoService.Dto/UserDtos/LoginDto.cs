using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.UserDtos;

public class LoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
