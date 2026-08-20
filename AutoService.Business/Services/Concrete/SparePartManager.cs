using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.SparePartDtos;
using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Concrete;

public class SparePartManager : ISparePartService
{
    private readonly ISparePartRepository _sparePartRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SparePartManager(
        ISparePartRepository sparePartRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _sparePartRepository = sparePartRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ResultSparePartDto>> GetAllAsync()
    {
        var values = await _sparePartRepository.GetAllAsync();
        return _mapper.Map<List<ResultSparePartDto>>(values);
    }

    public async Task<SparePartDetailDto?> GetByIdAsync(int id)
    {
        var value = await _sparePartRepository.GetByIdAsync(id);

        if (value == null)
            return null;

        return _mapper.Map<SparePartDetailDto>(value);
    }

    public async Task AddAsync(CreateSparePartDto dto)
    {
        var entity = _mapper.Map<SparePart>(dto);

        await _sparePartRepository.AddAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateSparePartDto dto)
    {
        var entity = await _sparePartRepository.GetByIdAsync(dto.SparePartId);

        if (entity == null)
            return;

        _mapper.Map(dto, entity);

        _sparePartRepository.Update(entity);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _sparePartRepository.GetByIdAsync(id);

        if (entity == null)
            return;

        await _sparePartRepository.SoftDeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();
    }
}