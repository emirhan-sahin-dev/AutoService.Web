using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.CustomerDtos;

public class ResultCustomerDto
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;
}
