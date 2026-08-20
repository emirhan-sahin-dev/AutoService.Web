using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<List<Role>> GetAllWithUsersAsync();

    Task<Role?> GetByIdWithUsersAsync(int id);

    Task<bool> RoleNameExistsAsync(
        string roleName,
        int? excludedRoleId = null);

    Task<int> GetUserCountAsync(int roleId);
}