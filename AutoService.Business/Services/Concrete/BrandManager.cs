using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.BrandDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class BrandManager : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public BrandManager(IBrandRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<List<ResultBrandDto>> GetAllAsync()
    {
        var values = await _brandRepository.GetAllAsync();
        return _mapper.Map<List<ResultBrandDto>>(values);
    }

    public async Task<GetByIdBrandDto> GetByIdAsync(int id)
    {
        var value = await _brandRepository.GetByIdAsync(id);
        return _mapper.Map<GetByIdBrandDto>(value);
    }

    public async Task AddAsync(CreateBrandDto dto)
    {
        var value = _mapper.Map<Brand>(dto);
        await _brandRepository.AddAsync(value);
    }

    public async Task UpdateAsync(UpdateBrandDto dto)
    {
        var value = await _brandRepository.GetByIdAsync(dto.BrandId);

        if (value == null)
        {
            return;
        }

        value.BrandName = dto.BrandName;

        _brandRepository.Update(value);
    }

    public async Task DeleteAsync(int id)
    {
        var value = await _brandRepository.GetByIdAsync(id);

        if (value != null)
        {
            await _brandRepository.SoftDeleteAsync(value);
        }
    }
}