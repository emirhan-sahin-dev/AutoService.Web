using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoService.Dto.PaymentDtos;

public class CreatePaymentDto
{
    public int ServiceRecordId { get; set; }

    [Display(Name = "Ödeme Tutarı")]
    [Range(
        0.01,
        10000000,
        ErrorMessage = "Ödeme tutarı 0'dan büyük olmalıdır.")]
    public decimal Amount { get; set; }

    [Display(Name = "Ödeme Yöntemi")]
    [Required(ErrorMessage = "Ödeme yöntemi seçiniz.")]
    public PaymentMethod PaymentMethod { get; set; }

    [Display(Name = "Ödeme Tarihi")]
    [Required(ErrorMessage = "Ödeme tarihi zorunludur.")]
    public DateTime PaymentDate { get; set; } = DateTime.Now;

    [Display(Name = "Açıklama")]
    [StringLength(
        500,
        ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; set; }

    [Display(Name = "İşlem Referansı")]
    [StringLength(
        100,
        ErrorMessage = "İşlem referansı en fazla 100 karakter olabilir.")]
    public string? TransactionReference { get; set; }
}
