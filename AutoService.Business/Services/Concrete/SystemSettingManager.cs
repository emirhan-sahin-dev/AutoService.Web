using AutoService.Business.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.DataAccess.Repositories.Interfaces;
using AutoService.Dto.SystemSettingDtos;

namespace AutoService.Business.Concrete
{
    public class SystemSettingManager : ISystemSettingService
    {
        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingManager(
            ISystemSettingRepository systemSettingRepository,
            IUnitOfWork unitOfWork)
        {
            _systemSettingRepository = systemSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultSystemSettingDto?> GetAsync()
        {
            var setting = await _systemSettingRepository.GetAsync();

            if (setting == null)
            {
                return null;
            }

            return new ResultSystemSettingDto
            {
                SystemSettingId = setting.SystemSettingId,
                CompanyName = setting.CompanyName,
                CompanyPhone = setting.CompanyPhone,
                CompanyEmail = setting.CompanyEmail,
                CompanyAddress = setting.CompanyAddress,
                VatRate = setting.VatRate,
                CriticalStockLevel = setting.CriticalStockLevel,
                SessionTimeoutMinutes = setting.SessionTimeoutMinutes,
                Currency = setting.Currency
            };
        }

        public async Task UpdateAsync(UpdateSystemSettingDto dto)
        {
            var setting = await _systemSettingRepository.GetAsync();

            if (setting == null)
            {
                throw new Exception("Sistem ayarları kaydı bulunamadı.");
            }

            setting.CompanyName = dto.CompanyName;
            setting.CompanyPhone = dto.CompanyPhone;
            setting.CompanyEmail = dto.CompanyEmail;
            setting.CompanyAddress = dto.CompanyAddress;
            setting.VatRate = dto.VatRate;
            setting.CriticalStockLevel = dto.CriticalStockLevel;
            setting.SessionTimeoutMinutes = dto.SessionTimeoutMinutes;
            setting.Currency = dto.Currency;
            setting.UpdatedDate = DateTime.Now;

            await _systemSettingRepository.UpdateAsync(setting);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}