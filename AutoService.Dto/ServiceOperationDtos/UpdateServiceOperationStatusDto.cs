using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceOperationDtos;

public class UpdateServiceOperationStatusDto
{
    public int ServiceOperationId { get; set; }

    [Display(Name = "İşlem Durumu")]
    [Required(ErrorMessage = "İşlem durumu seçiniz.")]
    public ServiceOperationStatus Status { get; set; }

    [Display(Name = "Yapılan İşlem Açıklaması")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla 1000 karakter olabilir.")]
    public string? WorkDescription { get; set; }
}
