using AutoService.Dto.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.UserDtos;

namespace AutoService.Business.Services.Abstract;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);

    Task<List<ResultUserDto>> GetAllAsync();

    Task<List<ResultUserDto>> SearchAsync(string keyword);

    Task<GetByIdUserDto?> GetByIdAsync(int id);

    Task AddAsync(CreateUserDto dto);

    Task UpdateAsync(UpdateUserDto dto);

    Task DeleteAsync(int id);

    Task ToggleStatusAsync(int id);
    Task ChangePasswordAsync(ChangePasswordDto dto);
    Task<PagedResultDto<ResultUserDto>> GetPagedAsync(
    int page,
    int pageSize,
    string? keyword = null);
}