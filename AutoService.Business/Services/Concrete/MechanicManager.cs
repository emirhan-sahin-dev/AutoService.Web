using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.MechanicDtos;
using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Concrete;

public class MechanicManager : IMechanicService
{
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public MechanicManager(
        IMechanicRepository mechanicRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mechanicRepository = mechanicRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ResultMechanicDto>> GetAllAsync()
    {
        var values = await _mechanicRepository.GetAllWithDetailsAsync();
        return _mapper.Map<List<ResultMechanicDto>>(values);
    }

    public async Task<MechanicDetailDto?> GetByIdAsync(int id)
    {
        var value = await _mechanicRepository.GetByIdWithDetailsAsync(id);

        if (value == null)
            return null;

        return _mapper.Map<MechanicDetailDto>(value);
    }

    public async Task AddAsync(CreateMechanicDto dto)
    {
        var entity = _mapper.Map<Mechanic>(dto);

        await _mechanicRepository.AddAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateMechanicDto dto)
    {
        var entity = await _mechanicRepository.GetByIdAsync(dto.MechanicId);

        if (entity == null)
            return;

        _mapper.Map(dto, entity);

        _mechanicRepository.Update(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _mechanicRepository.GetByIdAsync(id);

        if (entity == null)
            return;

        await _mechanicRepository.SoftDeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }
}
