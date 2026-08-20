using AutoService.Dto.MechanicDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Abstract;

public interface IMechanicService
{
    Task<List<ResultMechanicDto>> GetAllAsync();

    Task<MechanicDetailDto?> GetByIdAsync(int id);

    Task AddAsync(CreateMechanicDto dto);

    Task UpdateAsync(UpdateMechanicDto dto);

    Task DeleteAsync(int id);
}