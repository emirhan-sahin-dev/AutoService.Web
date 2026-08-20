using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;

namespace AutoService.DataAccess.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly AutoServiceContext _context;
        public UnitOfWork(AutoServiceContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
