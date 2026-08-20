using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.BrandDtos;

namespace AutoService.Business.Services.Abstract;

public interface IBrandService
{
    Task<List<ResultBrandDto>> GetAllAsync();

    Task<GetByIdBrandDto> GetByIdAsync(int id);

    Task AddAsync(CreateBrandDto dto);

    Task UpdateAsync(UpdateBrandDto dto);

    Task DeleteAsync(int id);
}
