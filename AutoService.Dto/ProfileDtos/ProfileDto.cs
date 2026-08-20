using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ProfileDtos
{
    public class ProfileDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Email { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
