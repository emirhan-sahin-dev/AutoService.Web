using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceOperationDtos;

public class CreateServiceOperationBatchDto
{
    [Display(Name = "Servis Kaydı")]
    [Range(1, int.MaxValue, ErrorMessage = "Servis kaydı seçiniz.")]
    public int ServiceRecordId { get; set; }

    public List<CreateServiceOperationItemDto> Operations { get; set; }
        = new();
}
