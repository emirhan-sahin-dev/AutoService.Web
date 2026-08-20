using AutoService.Dto.SparePartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Abstract;

public interface ISparePartService
{
    Task<List<ResultSparePartDto>> GetAllAsync();
    Task<SparePartDetailDto?> GetByIdAsync(int id);
    Task AddAsync(CreateSparePartDto dto);
    Task UpdateAsync(UpdateSparePartDto dto);
    Task DeleteAsync(int id);
}
