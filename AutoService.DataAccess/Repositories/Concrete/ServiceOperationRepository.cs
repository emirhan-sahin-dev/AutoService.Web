using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceOperationRepository
    : GenericRepository<ServiceOperation>,
      IServiceOperationRepository
{
    public ServiceOperationRepository(
        AutoServiceContext context)
        : base(context)
    {
    }

    public async Task<List<ServiceOperation>>
        GetOperationsByServiceRecordAsync(
            int serviceRecordId)
    {
        return await _context.ServiceOperations
            .AsNoTracking()

            .Include(x => x.ServiceRecord)
                .ThenInclude(x => x.Vehicle)

            .Include(x => x.ServiceOperationType)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.Mechanic)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.ServiceOperationParts)
                .ThenInclude(x => x.SparePart)

            .Where(x =>
                x.ServiceRecordId == serviceRecordId &&
                !x.IsDeleted)

            .OrderBy(x => x.ServiceOperationId)

            .ToListAsync();
    }

    public async Task<ServiceOperation?>
        GetOperationWithPartsAsync(
            int serviceOperationId)
    {
        return await _context.ServiceOperations
            .AsNoTracking()

            .Include(x => x.ServiceRecord)
                .ThenInclude(x => x.Vehicle)

            .Include(x => x.ServiceOperationType)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.Mechanic)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.ServiceOperationParts)
                .ThenInclude(x => x.SparePart)

            .FirstOrDefaultAsync(x =>
                x.ServiceOperationId ==
                    serviceOperationId &&
                !x.IsDeleted);
    }

    public async Task<List<ServiceOperation>>
        GetOperationsWithDetailsAsync()
    {
        return await _context.ServiceOperations
            .AsNoTracking()

            .Include(x => x.ServiceRecord)
                .ThenInclude(x => x.Vehicle)

            .Include(x => x.ServiceOperationType)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.Mechanic)
                .ThenInclude(x => x.MechanicSpecialty)

            .Include(x => x.ServiceOperationParts)
                .ThenInclude(x => x.SparePart)

            .Where(x => !x.IsDeleted)

            .OrderByDescending(x => x.CreatedDate)

            .ToListAsync();
    }
}