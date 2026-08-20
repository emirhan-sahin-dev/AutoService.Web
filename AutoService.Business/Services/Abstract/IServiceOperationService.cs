using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.ServiceOperationDtos;

namespace AutoService.Business.Services.Abstract;

public interface IServiceOperationService
{
    Task<List<ResultServiceOperationDto>>
        GetAllAsync();

    Task<List<ResultServiceOperationDto>>
        GetByServiceRecordIdAsync(
            int serviceRecordId);

    Task<ServiceOperationDetailDto?>
        GetByIdAsync(int id);

    Task AddAsync(
        CreateServiceOperationDto dto);

    Task AddBatchAsync(
        CreateServiceOperationBatchDto dto);

    Task UpdateStatusAsync(
        UpdateServiceOperationStatusDto dto);

    Task DeleteAsync(int id);

    // Servis işlemine yedek parça ekler ve stoktan düşer.
    Task AddPartAsync(
        AddServiceOperationPartDto dto);

    // Kullanılan parçayı kaldırır ve stoğa geri ekler.
    Task RemovePartAsync(
        int serviceOperationPartId);
}
