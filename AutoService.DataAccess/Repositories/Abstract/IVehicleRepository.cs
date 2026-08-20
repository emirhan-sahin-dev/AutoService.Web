using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;


namespace AutoService.DataAccess.Repositories.Abstract;

public interface IVehicleRepository : IGenericRepository<Vehicle>
{
    Task<List<Vehicle>> GetVehiclesWithDetailsAsync();

    Task<Vehicle?> GetVehicleWithDetailsAsync(int vehicleId);
    Task<List<Customer>> GetCustomersAsync();
    Task<List<Brand>> GetBrandsAsync();
    Task<List<Model>> GetModelsAsync();
}
