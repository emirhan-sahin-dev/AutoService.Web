using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.SparePartDtos;

public class ResultSparePartDto
{
    public int SparePartId { get; set; }
    public int ServiceDetailId { get; set; }
    public string PartName { get; set; } = null!;
    public string PartCode { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }

}
