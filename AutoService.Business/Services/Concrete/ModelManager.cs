using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.ModelDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class ModelManager : IModelService
{
    private readonly IModelRepository _modelRepository;
    private readonly IMapper _mapper;

    public ModelManager(IModelRepository modelRepository, IMapper mapper)
    {
        _modelRepository = modelRepository;
        _mapper = mapper;
    }

    public async Task<List<ResultModelDto>> GetAllAsync()
    {
        var values = await _modelRepository.GetAllAsync();
        return _mapper.Map<List<ResultModelDto>>(values);
    }

    public async Task<GetByIdModelDto?> GetByIdAsync(int id)
    {
        var value = await _modelRepository.GetByIdAsync(id);

        if (value == null)
            return null;

        return _mapper.Map<GetByIdModelDto>(value);
    }

    public async Task AddAsync(CreateModelDto dto)
    {
        var value = _mapper.Map<Model>(dto);
        await _modelRepository.AddAsync(value);
    }

    public async Task UpdateAsync(UpdateModelDto dto)
    {
        var value = await _modelRepository.GetByIdAsync(dto.ModelId);

        if (value == null)
        {
            return;
        }

        value.ModelName = dto.ModelName;
        value.BrandId = dto.BrandId;

        _modelRepository.Update(value);
    }

    public async Task DeleteAsync(int id)
    {
        var value = await _modelRepository.GetByIdAsync(id);

        if (value != null)
        {
            await _modelRepository.SoftDeleteAsync(value);
        }
    }
    public async Task<List<ResultModelDto>> GetModelsByBrandIdAsync(int brandId)
    {
        var values = await _modelRepository.GetModelsByBrandIdAsync(brandId);

        return _mapper.Map<List<ResultModelDto>>(values);
    }
}
