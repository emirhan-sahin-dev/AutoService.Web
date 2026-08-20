using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities;

namespace AutoService.DataAccess.Repositories.Concrete;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    public BrandRepository(AutoServiceContext context) : base(context)
    {
    }
}
