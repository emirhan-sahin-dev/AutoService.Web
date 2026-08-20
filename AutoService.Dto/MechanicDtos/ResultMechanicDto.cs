using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.MechanicDtos;

public class ResultMechanicDto
{
    public int MechanicId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Specialty { get; set; } = null!;
    public string? Email { get; set; }
}
