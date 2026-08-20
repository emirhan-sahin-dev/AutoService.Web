using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoService.Entity.Entities.Base;

namespace AutoService.DataAccess.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task SoftDeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
