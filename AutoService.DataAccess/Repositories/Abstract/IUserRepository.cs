using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> LoginAsync(string username);

    Task<User?> GetUserWithRoleAsync(int id);

    Task<List<User>> GetAllWithRoleAsync();

    Task<List<User>> SearchWithRoleAsync(string keyword);

    Task<bool> UsernameExistsAsync(string username, int? excludedUserId = null);

    Task<bool> EmailExistsAsync(string email, int? excludedUserId = null);
    Task<(List<User> Items, int TotalCount)> GetPagedWithRoleAsync(
    int page,
    int pageSize,
    string? keyword = null);
    Task<User?> GetByIdWithRoleAsync(int id);
    Task UpdateAsync(User user);
}
