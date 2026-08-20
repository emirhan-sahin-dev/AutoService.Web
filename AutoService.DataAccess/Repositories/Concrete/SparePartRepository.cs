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

public class SparePartRepository : GenericRepository<SparePart>, ISparePartRepository
{
    public SparePartRepository(AutoServiceContext context) : base(context)
    {
    }
    public async Task<List<SparePart>> GetAllAsync()
    {
        return await _context.SpareParts
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<SparePart?> GetByIdAsync(int id)
    {
        return await _context.SpareParts
            .FirstOrDefaultAsync(x => x.SparePartId == id && !x.IsDeleted);
    }
}
