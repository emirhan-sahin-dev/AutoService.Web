using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.RoleDtos;

namespace AutoService.Business.Services.Abstract;

public interface IRoleService
{
    Task<List<ResultRoleDto>> GetAllAsync();

    Task<GetByIdRoleDto?> GetByIdAsync(int id);

    Task AddAsync(CreateRoleDto dto);

    Task UpdateAsync(UpdateRoleDto dto);

    Task DeleteAsync(int id);
}
