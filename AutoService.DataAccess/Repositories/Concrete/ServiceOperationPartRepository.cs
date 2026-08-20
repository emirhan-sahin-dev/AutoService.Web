using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceOperationPartRepository
    : GenericRepository<ServiceOperationPart>,
      IServiceOperationPartRepository
{
    public ServiceOperationPartRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<ServiceOperationPart?>
        GetByIdWithDetailsAsync(
            int serviceOperationPartId)
    {
        return await _context.ServiceOperationParts
            .Include(x => x.SparePart)
            .Include(x => x.ServiceOperation)
            .FirstOrDefaultAsync(x =>
                x.ServiceOperationPartId ==
                    serviceOperationPartId &&
                !x.IsDeleted);
    }

    public async Task<ServiceOperationPart?>
        GetByOperationAndSparePartAsync(
            int serviceOperationId,
            int sparePartId)
    {
        return await _context.ServiceOperationParts
            .Include(x => x.SparePart)
            .FirstOrDefaultAsync(x =>
                x.ServiceOperationId ==
                    serviceOperationId &&
                x.SparePartId ==
                    sparePartId &&
                !x.IsDeleted);
    }

    public async Task<ServiceOperationPart?>
        GetDeletedByOperationAndSparePartAsync(
            int serviceOperationId,
            int sparePartId)
    {
        return await _context.ServiceOperationParts
            .IgnoreQueryFilters()
            .Include(x => x.SparePart)
            .FirstOrDefaultAsync(x =>
                x.ServiceOperationId ==
                    serviceOperationId &&
                x.SparePartId ==
                    sparePartId &&
                x.IsDeleted);
    }
}