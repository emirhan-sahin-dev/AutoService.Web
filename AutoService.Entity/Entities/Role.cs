using AutoService.Entity.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities;

public class Role : BaseEntity
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public ICollection<User> Users { get; set; }
        = new List<User>();
    public bool IsActive { get; set; }
}
