using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.BrandDtos;

public class CreateBrandDto
{
    [Display(Name = "Marka Adı")]
    public string BrandName { get; set; } = null!;
}