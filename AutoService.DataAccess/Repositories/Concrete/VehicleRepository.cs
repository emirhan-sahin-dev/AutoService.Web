using AutoService.DataAccess.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using AutoService.DataAccess.Contexts;

namespace AutoService.DataAccess.Repositories.Concrete;

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AutoServiceContext context) : base(context)
    {
    }

    public async Task<List<Vehicle>> GetVehiclesWithDetailsAsync()
    {
        return await _context.Vehicles
            .Include(x => x.Customer)
            .Include(x => x.Model)
                .ThenInclude(x => x.Brand)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleWithDetailsAsync(int vehicleId)
    {
        return await _context.Vehicles
            .Include(x => x.Customer)
            .Include(x => x.Model)
                .ThenInclude(x => x.Brand)
            .FirstOrDefaultAsync(x => x.VehicleId == vehicleId && !x.IsDeleted);
    }
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task<List<Brand>> GetBrandsAsync()
    {
        return await _context.Brands
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.BrandName)
            .ToListAsync();
    }

    public async Task<List<Model>> GetModelsAsync()
    {
        return await _context.Models
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.ModelName)
            .ToListAsync();
    }
}
