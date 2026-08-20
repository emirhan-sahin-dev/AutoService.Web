using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IServiceRecordRepository : IGenericRepository<ServiceRecord>
{
    Task<List<ServiceRecord>> GetAllWithDetailsAsync();
    Task<ServiceRecord?> GetByIdWithDetailsAsync(int id);
}
