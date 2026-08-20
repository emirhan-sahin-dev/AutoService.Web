using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.BrandDtos;

public class GetByIdBrandDto
{
    public int BrandId { get; set; }
    public string BrandName { get; set; } = null!;
}