using AutoService.Dto.CommonDtos;
using AutoMapper;
using AutoService.Business.Exceptions;
using AutoService.Business.Security;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.UserDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class UserManager : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserManager(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.LoginAsync(dto.Username);

        if (user == null)
            throw new BusinessException("Kullanıcı adı veya şifre yanlış.");

        if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            throw new BusinessException("Kullanıcı adı veya şifre yanlış.");

        return new LoginResponseDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Role = user.Role.RoleName
        };
    }

    public async Task<List<ResultUserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllWithRoleAsync();

        return users.Select(x => new ResultUserDto
        {
            UserId = x.UserId,
            FullName = x.FullName,
            Username = x.Username,
            Email = x.Email,
            IsActive = x.IsActive,
            RoleId = x.RoleId,
            RoleName = x.Role.RoleName,
            CreatedDate = x.CreatedDate
        }).ToList();
    }

    public async Task<List<ResultUserDto>> SearchAsync(string keyword)
    {
        var users = await _userRepository.SearchWithRoleAsync(keyword);

        return users.Select(x => new ResultUserDto
        {
            UserId = x.UserId,
            FullName = x.FullName,
            Username = x.Username,
            Email = x.Email,
            IsActive = x.IsActive,
            RoleId = x.RoleId,
            RoleName = x.Role.RoleName,
            CreatedDate = x.CreatedDate
        }).ToList();
    }

    public async Task<GetByIdUserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetUserWithRoleAsync(id);

        if (user == null)
            return null;

        return new GetByIdUserDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task AddAsync(CreateUserDto dto)
    {
        if (await _userRepository.UsernameExistsAsync(dto.Username))
            throw new BusinessException("Bu kullanıcı adı zaten kullanılıyor.");

        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new BusinessException("Bu e-posta adresi zaten kullanılıyor.");

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Username = dto.Username.Trim(),
            Email = dto.Email.Trim(),
            PasswordHash = PasswordHasher.Hash(dto.Password),
            RoleId = dto.RoleId,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.Now,
            IsDeleted = false
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);

        if (user == null || user.IsDeleted)
            throw new BusinessException("Kullanıcı bulunamadı.");

        if (await _userRepository.UsernameExistsAsync(
                dto.Username,
                dto.UserId))
        {
            throw new BusinessException("Bu kullanıcı adı zaten kullanılıyor.");
        }

        if (await _userRepository.EmailExistsAsync(
                dto.Email,
                dto.UserId))
        {
            throw new BusinessException("Bu e-posta adresi zaten kullanılıyor.");
        }

        user.FullName = dto.FullName.Trim();
        user.Username = dto.Username.Trim();
        user.Email = dto.Email.Trim();
        user.RoleId = dto.RoleId;
        user.IsActive = dto.IsActive;
        user.UpdatedDate = DateTime.Now;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null || user.IsDeleted)
            throw new BusinessException("Kullanıcı bulunamadı.");

        user.IsDeleted = true;
        user.IsActive = false;
        user.DeletedDate = DateTime.Now;
        user.UpdatedDate = DateTime.Now;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null || user.IsDeleted)
            throw new BusinessException("Kullanıcı bulunamadı.");

        user.IsActive = !user.IsActive;
        user.UpdatedDate = DateTime.Now;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.UserId);

        if (user == null || user.IsDeleted)
            throw new BusinessException("Kullanıcı bulunamadı.");

        if (string.IsNullOrWhiteSpace(dto.NewPassword))
            throw new BusinessException("Yeni şifre boş bırakılamaz.");

        if (dto.NewPassword.Length < 6)
            throw new BusinessException("Şifre en az 6 karakter olmalıdır.");

        if (dto.NewPassword != dto.ConfirmPassword)
            throw new BusinessException("Şifreler birbiriyle uyuşmuyor.");

        user.PasswordHash = PasswordHasher.Hash(dto.NewPassword);
        user.UpdatedDate = DateTime.Now;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<PagedResultDto<ResultUserDto>> GetPagedAsync(
    int page,
    int pageSize,
    string? keyword = null)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        var result = await _userRepository.GetPagedWithRoleAsync(
            page,
            pageSize,
            keyword);

        var users = result.Items.Select(x => new ResultUserDto
        {
            UserId = x.UserId,
            FullName = x.FullName,
            Username = x.Username,
            Email = x.Email,
            IsActive = x.IsActive,
            RoleId = x.RoleId,
            RoleName = x.Role.RoleName,
            CreatedDate = x.CreatedDate
        }).ToList();

        return new PagedResultDto<ResultUserDto>
        {
            Items = users,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = result.TotalCount
        };
    }
}