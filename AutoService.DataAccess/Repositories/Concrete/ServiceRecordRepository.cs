using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceRecordRepository
    : GenericRepository<ServiceRecord>,
      IServiceRecordRepository
{
    public ServiceRecordRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<List<ServiceRecord>>
        GetAllWithDetailsAsync()
    {
        return await _context.ServiceRecords
            .AsNoTracking()

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Customer)

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.ServiceOperationParts)
                    .ThenInclude(x => x.SparePart)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.Mechanic)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.ServiceOperationType)

            .Where(x => !x.IsDeleted)

            .OrderByDescending(x => x.CheckInDate)

            .ToListAsync();
    }

    public async Task<ServiceRecord?>
        GetByIdWithDetailsAsync(int id)
    {
        return await _context.ServiceRecords

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Customer)

            .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                    .ThenInclude(x => x.Brand)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.ServiceOperationParts)
                    .ThenInclude(x => x.SparePart)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.Mechanic)

            .Include(x => x.ServiceOperations)
                .ThenInclude(x => x.ServiceOperationType)

            .FirstOrDefaultAsync(x =>
                x.ServiceRecordId == id &&
                !x.IsDeleted);

    }
}