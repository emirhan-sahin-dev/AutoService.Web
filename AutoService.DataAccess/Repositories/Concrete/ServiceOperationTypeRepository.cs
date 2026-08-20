using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceOperationTypeRepository
    : GenericRepository<ServiceOperationType>,
      IServiceOperationTypeRepository
{
    private readonly AutoServiceContext _context;

    public ServiceOperationTypeRepository(
        AutoServiceContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<ServiceOperationType>>
        GetAllWithSpecialtyAsync()
    {
        return await _context.ServiceOperationTypes
            .AsNoTracking()
            .Include(x => x.MechanicSpecialty)
            .Where(x => !x.IsDeleted && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<ServiceOperationType?>
        GetByIdWithSpecialtyAsync(int id)
    {
        return await _context.ServiceOperationTypes
            .AsNoTracking()
            .Include(x => x.MechanicSpecialty)
            .FirstOrDefaultAsync(x =>
                x.ServiceOperationTypeId == id &&
                !x.IsDeleted &&
                x.IsActive);
    }
}