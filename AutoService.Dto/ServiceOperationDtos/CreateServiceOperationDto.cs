using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.ServiceOperationDtos;

public class CreateServiceOperationDto
{
    [Display(Name = "Servis Kaydı")]
    [Range(1, int.MaxValue, ErrorMessage = "Servis kaydı seçiniz.")]
    public int ServiceRecordId { get; set; }

    [Display(Name = "İşlem Türü")]
    [Range(1, int.MaxValue, ErrorMessage = "İşlem türü seçiniz.")]
    public int ServiceOperationTypeId { get; set; }

    [Display(Name = "Atanacak Usta")]
    [Range(1, int.MaxValue, ErrorMessage = "Usta seçiniz.")]
    public int MechanicId { get; set; }

    [Display(Name = "Tespit Edilen Problem")]
    [Required(ErrorMessage = "{0} alanı zorunludur.")]
    [StringLength(
        1000,
        MinimumLength = 5,
        ErrorMessage = "{0} 5 ile 1000 karakter arasında olmalıdır.")]
    public string ProblemDescription { get; set; } = null!;

    [Display(Name = "Yapılacak İşlem Açıklaması")]
    [StringLength(
        1000,
        ErrorMessage = "{0} en fazla 1000 karakter olabilir.")]
    public string? WorkDescription { get; set; }
}