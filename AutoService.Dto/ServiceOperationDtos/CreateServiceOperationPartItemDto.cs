using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceOperationDtos;

public class CreateServiceOperationPartItemDto
{
    [Display(Name = "Yedek Parça")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Yedek parça seçiniz.")]
    public int SparePartId { get; set; }

    [Display(Name = "Adet")]
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Parça adedi en az 1 olmalıdır.")]
    public int Quantity { get; set; } = 1;
}