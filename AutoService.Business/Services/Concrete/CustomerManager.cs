using AutoMapper;
using AutoService.Business.Exceptions;
using AutoService.Business.Services.Abstract;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Dto.CustomerDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Services.Concrete;

public class CustomerManager : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerManager(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ResultCustomerDto>> GetAllAsync()
    {
        var values = await _customerRepository.GetAllAsync();

        return _mapper.Map<List<ResultCustomerDto>>(values);
    }

    public async Task<CustomerDetailDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetCustomerWithVehiclesAsync(id);

        return _mapper.Map<CustomerDetailDto>(customer);
    }

    public async Task<CustomerDetailDto?> GetCustomerWithVehiclesAsync(int id)
    {
        var value = await _customerRepository.GetCustomerWithVehiclesAsync(id);

        if (value == null)
            return null;

        return _mapper.Map<CustomerDetailDto>(value);
    }

    public async Task AddAsync(CreateCustomerDto dto)
    {
        if (await _customerRepository.AnyAsync(x => x.Email == dto.Email))
            throw new BusinessException("Bu e-posta adresi zaten kayıtlı.");

        var customer = _mapper.Map<Customer>(dto);

        await _customerRepository.AddAsync(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);

        if (customer == null)
            throw new NotFoundException("Müşteri bulunamadı.");

        _mapper.Map(dto, customer);
        customer.UpdatedDate = DateTime.UtcNow;

        _customerRepository.Update(customer);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
            throw new NotFoundException("Müşteri bulunamadı.");

        await _customerRepository.SoftDeleteAsync(customer);

        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<List<ResultCustomerDto>> SearchAsync(string keyword)
    {
        var customers = await _customerRepository.SearchAsync(keyword);

        return _mapper.Map<List<ResultCustomerDto>>(customers);
    }
    public async Task<List<ResultCustomerDto>> GetPagedAsync(int page, int pageSize)
    {
        var values = await _customerRepository.GetPagedAsync(page, pageSize);
        return _mapper.Map<List<ResultCustomerDto>>(values);
    }
    public async Task<int> GetCountAsync()
    {
        return await _customerRepository.GetCountAsync();
    }
}