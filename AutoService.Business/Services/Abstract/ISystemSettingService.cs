using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.SystemSettingDtos;

namespace AutoService.Business.Abstract
{
    public interface ISystemSettingService
    {
        Task<ResultSystemSettingDto?> GetAsync();

        Task UpdateAsync(UpdateSystemSettingDto dto);
    }
}
