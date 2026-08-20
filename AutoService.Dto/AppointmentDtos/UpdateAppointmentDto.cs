using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.AppointmentDtos;

public class UpdateAppointmentDto
{
    public int AppointmentId { get; set; }

    [Display(Name = "Müşteri")]
    [Range(1, int.MaxValue, ErrorMessage = "Müşteri seçiniz.")]
    public int CustomerId { get; set; }

    [Display(Name = "Araç")]
    [Range(1, int.MaxValue, ErrorMessage = "Araç seçiniz.")]
    public int VehicleId { get; set; }

    [Display(Name = "Randevu Tarihi")]
    [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
    public DateTime AppointmentDate { get; set; }

    [Display(Name = "Müşteri Talebi")]
    [Required(ErrorMessage = "Müşteri talebi zorunludur.")]
    [StringLength(
        1000,
        MinimumLength = 5,
        ErrorMessage = "Müşteri talebi 5 ile 1000 karakter arasında olmalıdır.")]
    public string CustomerRequest { get; set; } = null!;

    [Display(Name = "Açıklama")]
    [StringLength(
        1000,
        ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    public string? Description { get; set; }

    [Display(Name = "Randevu Durumu")]
    public AppointmentStatus Status { get; set; }
}
