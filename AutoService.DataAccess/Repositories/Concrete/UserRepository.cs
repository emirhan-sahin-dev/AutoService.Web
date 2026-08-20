using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AutoServiceContext _context;

    public UserRepository(AutoServiceContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<User?> LoginAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x =>
                x.Username == username &&
                x.IsActive &&
                !x.IsDeleted);
    }

    public async Task<User?> GetUserWithRoleAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x =>
                x.UserId == id &&
                !x.IsDeleted);
    }

    public async Task<List<User>> GetAllWithRoleAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<User>> SearchWithRoleAsync(string keyword)
    {
        keyword = keyword.Trim();

        return await _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Where(x =>
                !x.IsDeleted &&
                (
                    x.FullName.Contains(keyword) ||
                    x.Username.Contains(keyword) ||
                    x.Email.Contains(keyword) ||
                    x.Role.RoleName.Contains(keyword)
                ))
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<bool> UsernameExistsAsync(
        string username,
        int? excludedUserId = null)
    {
        return await _context.Users.AnyAsync(x =>
            !x.IsDeleted &&
            x.Username == username &&
            (!excludedUserId.HasValue || x.UserId != excludedUserId.Value));
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        int? excludedUserId = null)
    {
        return await _context.Users.AnyAsync(x =>
            !x.IsDeleted &&
            x.Email == email &&
            (!excludedUserId.HasValue || x.UserId != excludedUserId.Value));
    }
    public async Task<(List<User> Items, int TotalCount)>
    GetPagedWithRoleAsync(
        int page,
        int pageSize,
        string? keyword = null)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        var query = _context.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();

            query = query.Where(x =>
                x.FullName.Contains(keyword) ||
                x.Username.Contains(keyword) ||
                x.Email.Contains(keyword) ||
                x.Role.RoleName.Contains(keyword));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public async Task<User?> GetByIdWithRoleAsync(int id)
    {
        return await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.UserId == id && !x.IsDeleted);
    }
    public Task UpdateAsync(User user)
    {
        _context.Users.Update(user);

        return Task.CompletedTask;
    }
}