using AutoService.Dto.CustomerDtos;

namespace AutoService.Business.Services.Abstract;

public interface ICustomerService
{
    Task<List<ResultCustomerDto>> GetAllAsync();

    Task<CustomerDetailDto?> GetByIdAsync(int id);

    Task AddAsync(CreateCustomerDto dto);

    Task UpdateAsync(UpdateCustomerDto dto);

    Task DeleteAsync(int id);
    Task<List<ResultCustomerDto>> SearchAsync(string keyword);
    Task<List<ResultCustomerDto>> GetPagedAsync(int page, int pageSize);
    Task<int> GetCountAsync();
}