using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Dto.ModelDtos;

namespace AutoService.Business.Services.Abstract;

public interface IModelService
{
    Task<List<ResultModelDto>> GetAllAsync();
    Task<GetByIdModelDto?> GetByIdAsync(int id);
    Task AddAsync(CreateModelDto dto);
    Task UpdateAsync(UpdateModelDto dto);
    Task DeleteAsync(int id);
    Task<List<ResultModelDto>> GetModelsByBrandIdAsync(int brandId);
}


