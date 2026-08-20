using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.DataAccess.Repositories.Concrete;

public class ServiceDetailRepository : GenericRepository<ServiceDetail>, IServiceDetailRepository
{
    public ServiceDetailRepository(AutoServiceContext context) : base(context)
    {
    }

    public async Task<List<ServiceDetail>> GetAllWithDetailsAsync()
    {
        return await _context.ServiceDetails
            .Include(x => x.ServiceRecord)
                .ThenInclude(x => x.Vehicle)
            .Include(x => x.SparePart)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<ServiceDetail?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.ServiceDetails
            .Include(x => x.ServiceRecord)
                .ThenInclude(x => x.Vehicle)
            .Include(x => x.SparePart)
            .FirstOrDefaultAsync(x => x.ServiceDetailId == id && !x.IsDeleted);
    }
    public async Task<List<ServiceDetail>> GetByServiceRecordIdAsync(int serviceRecordId)
    {
        return await _context.ServiceDetails
            .Where(x => x.ServiceRecordId == serviceRecordId && !x.IsDeleted)
            .ToListAsync();
    }
}
