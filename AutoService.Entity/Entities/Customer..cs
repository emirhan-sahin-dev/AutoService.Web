using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class Customer : BaseEntity
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public ICollection<Appointment> Appointments { get; set; }
    = new List<Appointment>();
}