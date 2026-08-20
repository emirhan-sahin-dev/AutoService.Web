using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerWithVehiclesAsync(int customerId);
        Task<List<Customer>> GetCustomersWithVehiclesAsync();
        Task<List<Customer>> SearchAsync(string keyword);
        Task<List<Customer>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}
