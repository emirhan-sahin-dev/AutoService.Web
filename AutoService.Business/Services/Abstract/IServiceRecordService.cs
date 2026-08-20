using AutoService.Dto.ServiceRecordDtos;
using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Abstract;

public interface IServiceRecordService
{
    Task<List<ResultServiceRecordDto>> GetAllAsync();

    Task<ServiceRecordDetailDto?> GetByIdAsync(int id);

    Task AddAsync(CreateServiceRecordDto dto);

    Task UpdateAsync(UpdateServiceRecordDto dto);

    Task DeleteAsync(int id);
    Task DeliverVehicleAsync(int serviceRecordId);

    Task<ServiceAcceptanceFormDto?>
    GetAcceptanceFormAsync(int id);

    Task<ServiceExitReceiptDto?>
        GetExitReceiptAsync(int id);

}
