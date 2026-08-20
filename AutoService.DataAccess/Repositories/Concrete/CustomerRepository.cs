using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AutoServiceContext context) : base(context)
    {
    }

    public async Task<Customer?> GetCustomerWithVehiclesAsync(int customerId)
    {
        return await _context.Customers
             .Include(x => x.Vehicles)
             .FirstOrDefaultAsync(x => x.CustomerId == customerId && !x.IsDeleted);
    }

    public async Task<List<Customer>> GetCustomersWithVehiclesAsync()
    {
        return await _context.Customers
    .Where(x => !x.IsDeleted)
    .Include(x => x.Vehicles)
    .ToListAsync();
    }
    public async Task<List<Customer>> SearchAsync(string keyword)
    {
        return await _context.Customers
            .Where(x => !x.IsDeleted &&
                       x.FullName.Contains(keyword))
            .ToListAsync();
    }
    public async Task<List<Customer>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Customers
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.CustomerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<int> GetCountAsync()
    {
        return await _context.Customers
            .CountAsync(x => !x.IsDeleted);
    }
}
