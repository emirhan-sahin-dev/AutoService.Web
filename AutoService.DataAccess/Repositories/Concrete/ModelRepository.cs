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

public class ModelRepository : GenericRepository<Model>, IModelRepository
{
    public ModelRepository(AutoServiceContext context) : base(context)
    {
    }
    public async Task<List<Model>> GetModelsByBrandIdAsync(int brandId)
    {
        return await _context.Models
            .Where(x => x.BrandId == brandId && !x.IsDeleted)
            .ToListAsync();
    }
}
