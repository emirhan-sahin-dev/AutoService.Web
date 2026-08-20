using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract
{
    public interface IServiceOperationRepository : IGenericRepository<ServiceOperation>
    {
        Task<List<ServiceOperation>> GetOperationsByServiceRecordAsync(int serviceRecordId);

        Task<ServiceOperation?> GetOperationWithPartsAsync(int serviceOperationId);

        Task<List<ServiceOperation>> GetOperationsWithDetailsAsync();
    }
}