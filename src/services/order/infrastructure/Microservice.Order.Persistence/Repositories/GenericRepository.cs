using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Microservice.Order.Persistence.Repositories;

public class GenericRepository<TId, TEntity>(AppDbContext context) : IGenericRepository<TId,TEntity> where TId : struct where TEntity : BaseEntity<TId>
{
    protected AppDbContext Context = context; // Burayı sadece GenericRepository'den miras alacak olanlar kullanacak yani ProductRepository
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    
    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async ValueTask<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        // 1,10 => 1..10
        // 2,10 => 11..20
        return await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<bool> AnyAsync(TId id)
    {
        return await _dbSet.AnyAsync(x => x.Id.Equals(id));
    }
}