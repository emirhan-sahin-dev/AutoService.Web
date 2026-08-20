using AutoService.Dto.ServiceDetailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Abstract;

public interface IServiceDetailService
{
    Task<List<ResultServiceDetailDto>> GetAllAsync();

    Task<ServiceDetailDetailDto?> GetByIdAsync(int id);

    Task AddAsync(CreateServiceDetailDto dto);

    Task UpdateAsync(UpdateServiceDetailDto dto);

    Task DeleteAsync(int id);
}
