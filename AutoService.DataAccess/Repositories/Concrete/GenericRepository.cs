using AutoService.DataAccess.Contexts;
using AutoService.DataAccess.Repositories.Abstract;
using AutoService.Entity.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoService.DataAccess.Repositories.Concrete;

public class GenericRepository<T>
    : IGenericRepository<T>
    where T : BaseEntity
{
    protected readonly AutoServiceContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(
        AutoServiceContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        entity.CreatedDate = DateTime.Now;
        entity.IsDeleted = false;

        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedDate = DateTime.Now;

        _dbSet.Update(entity);
    }

    public Task SoftDeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        entity.DeletedDate = DateTime.Now;
        entity.UpdatedDate = DateTime.Now;

        _dbSet.Update(entity);

        return Task.CompletedTask;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .AnyAsync(predicate);
    }

    public async Task<List<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .ToListAsync();
    }
}