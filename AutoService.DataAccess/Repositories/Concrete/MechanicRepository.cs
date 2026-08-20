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

public class MechanicRepository : GenericRepository<Mechanic>, IMechanicRepository
{
    public MechanicRepository(AutoServiceContext context) : base(context)
    {
    }

    public async Task<List<Mechanic>> GetAllWithDetailsAsync()
    {
        return await _context.Mechanics
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Mechanic?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Mechanics
            .FirstOrDefaultAsync(x => x.MechanicId == id && !x.IsDeleted);
    }
    public async Task<List<Mechanic>> GetMechanicsBySpecialtyIdAsync(
    int specialtyId)
    {
        return await _context.Mechanics
            .AsNoTracking()
            .Include(x => x.MechanicSpecialty)
            .Where(x =>
                x.MechanicSpecialtyId == specialtyId &&
                x.IsActive &&
                !x.IsDeleted)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync();
    }
}
