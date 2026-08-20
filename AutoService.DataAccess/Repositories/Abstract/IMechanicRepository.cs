using AutoService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.DataAccess.Repositories.Abstract;

public interface IMechanicRepository : IGenericRepository<Mechanic>
{
    Task<List<Mechanic>> GetAllWithDetailsAsync();

    Task<Mechanic?> GetByIdWithDetailsAsync(int id);
    Task<List<Mechanic>> GetMechanicsBySpecialtyIdAsync(int specialtyId);
}
