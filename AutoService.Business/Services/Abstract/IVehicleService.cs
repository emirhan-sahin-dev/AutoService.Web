using AutoService.Dto.VehicleDtos;
using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Abstract;

public interface IVehicleService
{
    Task<List<ResultVehicleDto>> GetAllAsync();
    Task<VehicleDetailDto?> GetByIdAsync(int id);
    Task AddAsync(CreateVehicleDto dto);
    Task UpdateAsync(UpdateVehicleDto dto);
    Task DeleteAsync(int id);

    Task<List<Customer>> GetCustomersAsync();
    Task<List<Brand>> GetBrandsAsync();
    Task<List<Model>> GetModelsAsync();
    Task<UpdateVehicleDto?> GetUpdateDtoAsync(int id);
}