using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceOperationTypeSparePartRepository
    : GenericRepository<ServiceOperationTypeSparePart>,
      IServiceOperationTypeSparePartRepository
{
    public ServiceOperationTypeSparePartRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<List<ServiceOperationTypeSparePart>>
        GetByOperationTypeIdAsync(
            int serviceOperationTypeId)
    {
        return await _context
            .ServiceOperationTypeSpareParts
            .AsNoTracking()

            .Include(x => x.SparePart)

            .Where(x =>
                x.ServiceOperationTypeId ==
                    serviceOperationTypeId &&
                !x.IsDeleted &&
                !x.SparePart.IsDeleted &&
                x.SparePart.StockQuantity > 0)

            .OrderBy(x =>
                x.SparePart.PartName)

            .ToListAsync();
    }
}