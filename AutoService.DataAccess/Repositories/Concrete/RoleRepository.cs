using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly AutoServiceContext _context;

    public RoleRepository(AutoServiceContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllWithUsersAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .Include(x => x.Users)
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.RoleName)
            .ToListAsync();
    }

    public async Task<Role?> GetByIdWithUsersAsync(int id)
    {
        return await _context.Roles
            .AsNoTracking()
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x =>
                x.RoleId == id &&
                !x.IsDeleted);
    }

    public async Task<bool> RoleNameExistsAsync(
        string roleName,
        int? excludedRoleId = null)
    {
        roleName = roleName.Trim();

        return await _context.Roles.AnyAsync(x =>
            !x.IsDeleted &&
            x.RoleName == roleName &&
            (!excludedRoleId.HasValue ||
             x.RoleId != excludedRoleId.Value));
    }

    public async Task<int> GetUserCountAsync(int roleId)
    {
        return await _context.Users.CountAsync(x =>
            x.RoleId == roleId &&
            !x.IsDeleted);
    }
}