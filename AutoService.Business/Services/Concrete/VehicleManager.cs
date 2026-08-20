using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.VehicleDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class VehicleManager : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VehicleManager(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<List<ResultVehicleDto>> GetAllAsync()
    {
        var values = await _vehicleRepository.GetVehiclesWithDetailsAsync();
        return _mapper.Map<List<ResultVehicleDto>>(values);
    }
    public async Task<VehicleDetailDto?> GetByIdAsync(int id)
    {
        var value = await _vehicleRepository.GetVehicleWithDetailsAsync(id);
        return _mapper.Map<VehicleDetailDto>(value);
    }
    public async Task AddAsync(CreateVehicleDto dto)
    {
        var entity = _mapper.Map<Vehicle>(dto);
        await _vehicleRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task UpdateAsync(UpdateVehicleDto dto)
    {
        var entity = _mapper.Map<Vehicle>(dto);
        _vehicleRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _vehicleRepository.GetByIdAsync(id);

        if (entity == null)
            return;

        await _vehicleRepository.SoftDeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _vehicleRepository.GetCustomersAsync();
    }

    public async Task<List<Brand>> GetBrandsAsync()
    {
        return await _vehicleRepository.GetBrandsAsync();
    }

    public async Task<List<Model>> GetModelsAsync()
    {
        return await _vehicleRepository.GetModelsAsync();
    }
    public async Task<UpdateVehicleDto?> GetUpdateDtoAsync(int id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);

        if (vehicle == null)
            return null;

        return _mapper.Map<UpdateVehicleDto>(vehicle);
    }
}
