using AutoMapper;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.ServiceDetailDtos;
using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.Business.Services.Concrete;

public class ServiceDetailManager : IServiceDetailService
{
    private readonly IServiceDetailRepository _serviceDetailRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISparePartRepository _sparePartRepository;
    private readonly IServiceRecordRepository _serviceRecordRepository;

    public ServiceDetailManager(
        IServiceDetailRepository serviceDetailRepository,
        ISparePartRepository sparePartRepository,
        IServiceRecordRepository serviceRecordRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _serviceDetailRepository = serviceDetailRepository;
        _sparePartRepository = sparePartRepository;
        _serviceRecordRepository = serviceRecordRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ResultServiceDetailDto>> GetAllAsync()
    {
        var values = await _serviceDetailRepository.GetAllWithDetailsAsync();
        return _mapper.Map<List<ResultServiceDetailDto>>(values);
    }

    public async Task<ServiceDetailDetailDto?> GetByIdAsync(int id)
    {
        var value = await _serviceDetailRepository.GetByIdWithDetailsAsync(id);

        if (value == null)
            return null;

        return _mapper.Map<ServiceDetailDetailDto>(value);
    }

    public async Task AddAsync(CreateServiceDetailDto dto)
    {
        var part = await _sparePartRepository.GetByIdAsync(dto.SparePartId);

        if (part == null)
            return;

        if (part.StockQuantity < dto.Quantity)
            throw new Exception("Yeterli stok bulunmuyor.");

        var entity = _mapper.Map<ServiceDetail>(dto);

        entity.UnitPrice = part.UnitPrice;
        entity.TotalPrice = dto.Quantity * part.UnitPrice;

        part.StockQuantity -= dto.Quantity;
        _sparePartRepository.Update(part);

        await _serviceDetailRepository.AddAsync(entity);

        await _unitOfWork.SaveChangesAsync();

        await UpdateServiceRecordTotalAsync(dto.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateServiceDetailDto dto)
    {
        var entity = await _serviceDetailRepository.GetByIdAsync(dto.ServiceDetailId);

        if (entity == null)
            return;

        var oldPart = await _sparePartRepository.GetByIdAsync(entity.SparePartId);

        if (oldPart != null)
        {
            oldPart.StockQuantity += entity.Quantity;
            _sparePartRepository.Update(oldPart);
        }

        var newPart = await _sparePartRepository.GetByIdAsync(dto.SparePartId);

        if (newPart == null)
            return;

        if (newPart.StockQuantity < dto.Quantity)
            throw new Exception("Yeterli stok bulunmuyor.");

        _mapper.Map(dto, entity);

        entity.UnitPrice = newPart.UnitPrice;
        entity.TotalPrice = dto.Quantity * newPart.UnitPrice;

        newPart.StockQuantity -= dto.Quantity;
        _sparePartRepository.Update(newPart);

        _serviceDetailRepository.Update(entity);

        await _unitOfWork.SaveChangesAsync();

        await UpdateServiceRecordTotalAsync(entity.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _serviceDetailRepository.GetByIdAsync(id);

        if (entity == null)
            return;

        var part = await _sparePartRepository.GetByIdAsync(entity.SparePartId);

        if (part != null)
        {
            part.StockQuantity += entity.Quantity;

            _sparePartRepository.Update(part);
        }

        await _serviceDetailRepository.SoftDeleteAsync(entity);

        await _unitOfWork.SaveChangesAsync();

        await UpdateServiceRecordTotalAsync(entity.ServiceRecordId);

        await _unitOfWork.SaveChangesAsync();
    }
    private async Task UpdateServiceRecordTotalAsync(int serviceRecordId)
    {
        var serviceRecord = await _serviceRecordRepository.GetByIdAsync(serviceRecordId);

        if (serviceRecord == null)
            return;

        var details = await _serviceDetailRepository
            .GetByServiceRecordIdAsync(serviceRecordId);

        decimal sparePartTotal = details.Sum(x => x.TotalPrice);

        serviceRecord.TotalPrice = serviceRecord.LaborCost + sparePartTotal;

        _serviceRecordRepository.Update(serviceRecord);
    }
}
