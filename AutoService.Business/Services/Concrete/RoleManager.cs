using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.RoleDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class RoleManager : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    private readonly IUnitOfWork _unitOfWork;

    public RoleManager(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ResultRoleDto>> GetAllAsync()
    {
        var values =
            await _roleRepository.GetAllWithUsersAsync();

        return values.Select(x => new ResultRoleDto
        {
            RoleId = x.RoleId,
            RoleName = x.RoleName,
            UserCount = x.Users.Count(y => !y.IsDeleted),
            CreatedDate = x.CreatedDate,
            IsActive = x.IsActive
        }).ToList();
    }

    public async Task<GetByIdRoleDto?> GetByIdAsync(int id)
    {
        var value =
            await _roleRepository.GetByIdWithUsersAsync(id);

        if (value == null)
            return null;

        return new GetByIdRoleDto
        {
            RoleId = value.RoleId,
            RoleName = value.RoleName,
            UserCount = value.Users.Count(y => !y.IsDeleted),
            CreatedDate = value.CreatedDate,
            IsActive = value.IsActive
        };
    }

    public async Task AddAsync(CreateRoleDto dto)
    {
        if (await _roleRepository.RoleNameExistsAsync(dto.RoleName))
            throw new Exception("Bu rol zaten mevcut.");

        var role = new Role
        {
            RoleName = dto.RoleName.Trim(),
            IsActive = true
        };

        await _roleRepository.AddAsync(role);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateRoleDto dto)
    {
        if (await _roleRepository.RoleNameExistsAsync(
            dto.RoleName,
            dto.RoleId))
        {
            throw new Exception("Bu rol adı kullanılmaktadır.");
        }

        var role =
            await _roleRepository.GetByIdAsync(dto.RoleId);

        if (role == null)
            throw new Exception("Rol bulunamadı.");

        role.RoleName = dto.RoleName.Trim();

        role.UpdatedDate = DateTime.Now;

        _roleRepository.Update(role);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var count =
            await _roleRepository.GetUserCountAsync(id);

        if (count > 0)
            throw new Exception(
                "Bu role bağlı kullanıcılar olduğu için silinemez.");

        var role =
            await _roleRepository.GetByIdAsync(id);

        if (role == null)
            throw new Exception("Rol bulunamadı.");

        role.IsDeleted = true;

        role.UpdatedDate = DateTime.Now;

        _roleRepository.Update(role);

        await _unitOfWork.SaveChangesAsync();
    }
}