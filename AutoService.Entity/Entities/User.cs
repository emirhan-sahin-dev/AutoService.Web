using AutoService.Entity.Entities.Base;

namespace AutoService.Entity.Entities;

public class User : BaseEntity
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
}