using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.ModelDtos;

public class GetByIdModelDto
{
    public int ModelId { get; set; }
    public string ModelName { get; set; } = null!;
    public int BrandId { get; set; }
}
