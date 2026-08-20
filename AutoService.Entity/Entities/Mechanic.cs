using AutoService.Entity.Entities.Base;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Entity.Entities;

public class Mechanic : BaseEntity
{
    public int MechanicId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    /*
     * GEÇİCİ OLARAK KALIYOR.
     * Yeni uzmanlık sistemi tamamlanınca kaldırılacak.
     */
    public string Specialty { get; set; } = null!;

    /*
     * İlk migration sırasında nullable bırakıyoruz.
     * Mevcut ustaların uzmanlıklarını atadıktan sonra
     * zorunlu hâle getirebiliriz.
     */
    public int? MechanicSpecialtyId { get; set; }

    public MechanicSpecialty? MechanicSpecialty { get; set; }

    public DateTime HireDate { get; set; }

    public bool IsActive { get; set; } = true;

    /*
     * Yeni sistemde ustaya atanan gerçek işlemler.
     */
    public ICollection<ServiceOperation> ServiceOperations { get; set; }
        = new List<ServiceOperation>();
}