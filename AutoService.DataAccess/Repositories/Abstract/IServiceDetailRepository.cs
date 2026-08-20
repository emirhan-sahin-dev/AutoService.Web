using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IServiceDetailRepository : IGenericRepository<ServiceDetail>
{
    Task<List<ServiceDetail>> GetAllWithDetailsAsync();

    Task<ServiceDetail?> GetByIdWithDetailsAsync(int id);

    Task<List<ServiceDetail>> GetByServiceRecordIdAsync(int serviceRecordId);
}