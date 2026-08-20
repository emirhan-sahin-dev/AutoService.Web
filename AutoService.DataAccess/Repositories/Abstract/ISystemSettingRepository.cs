using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Interfaces
{
    public interface ISystemSettingRepository
    {
        Task<SystemSetting?> GetAsync();

        Task UpdateAsync(SystemSetting setting);
    }
}
