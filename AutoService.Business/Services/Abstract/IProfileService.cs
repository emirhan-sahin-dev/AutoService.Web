using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.ProfileDtos;

namespace AutoService.Business.Abstract
{
    public interface IProfileService
    {
        Task<ProfileDto?> GetProfileAsync(int userId);
    }
}
