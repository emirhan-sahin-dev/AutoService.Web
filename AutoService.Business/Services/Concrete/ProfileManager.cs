using AutoService.Business.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.DataAccess.Repositories.Interfaces;
using AutoService.Dto.ProfileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Concrete
{
    public class ProfileManager : IProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ProfileDto?> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new ProfileDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                RoleName = user.Role != null
                    ? user.Role.RoleName
                    : "Rol bulunamadı"
            };
        }
    }
}