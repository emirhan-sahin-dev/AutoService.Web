using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Interfaces;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories
{
    public class SystemSettingRepository : ISystemSettingRepository
    {
        private readonly AutoServiceContext _context;

        public SystemSettingRepository(AutoServiceContext context)
        {
            _context = context;
        }

        public async Task<SystemSetting?> GetAsync()
        {
            return await _context.SystemSettings
                .FirstOrDefaultAsync(x => !x.IsDeleted);
        }

        public Task UpdateAsync(SystemSetting setting)
        {
            _context.SystemSettings.Update(setting);

            return Task.CompletedTask;
        }
    }
}