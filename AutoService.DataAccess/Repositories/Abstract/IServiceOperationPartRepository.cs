using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IServiceOperationPartRepository
    : IGenericRepository<ServiceOperationPart>
{
    Task<ServiceOperationPart?>
        GetByIdWithDetailsAsync(
            int serviceOperationPartId);

    Task<ServiceOperationPart?>
        GetByOperationAndSparePartAsync(
            int serviceOperationId,
            int sparePartId);

    Task<ServiceOperationPart?>
        GetDeletedByOperationAndSparePartAsync(
            int serviceOperationId,
            int sparePartId);
}