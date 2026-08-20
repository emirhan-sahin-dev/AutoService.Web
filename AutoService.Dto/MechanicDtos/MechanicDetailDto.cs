using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Dto.MechanicDtos;

public class MechanicDetailDto
{

    public int MechanicId { get; set; }

    [Display(Name = "Ad")]
    public string FirstName { get; set; }

    [Display(Name = "Soyad")]
    public string LastName { get; set; }

    [Display(Name = "Telefon")]
    public string Phone { get; set; }

    [Display(Name = "E-Posta")]
    public string Email { get; set; }

    [Display(Name = "Uzmanlık Alanı")]
    public string Specialty { get; set; }

    [Display(Name = "İşe Giriş Tarihi")]
    public DateTime HireDate { get; set; }
}
