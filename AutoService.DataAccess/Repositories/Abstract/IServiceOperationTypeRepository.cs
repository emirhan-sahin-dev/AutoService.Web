using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IServiceOperationTypeRepository
    : IGenericRepository<ServiceOperationType>
{
    Task<List<ServiceOperationType>> GetAllWithSpecialtyAsync();

    Task<ServiceOperationType?> GetByIdWithSpecialtyAsync(int id);
}
