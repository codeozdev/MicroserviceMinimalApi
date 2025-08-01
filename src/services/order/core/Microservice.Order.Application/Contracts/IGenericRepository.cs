using Microservice.Order.Domain.Entities;
using System.Linq.Expressions;

namespace Microservice.Order.Application.Contracts;

public interface IGenericRepository<TId, TEntity> where TId : struct where TEntity : BaseEntity<TId>
{
    Task<List<TEntity>> GetAllAsync();
    ValueTask<TEntity?> GetByIdAsync(TId id);

    Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    // Belirli bir şarta göre filtrelenmiş sorguya olanak tanır (IQueryable döner)
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    // Belirtilen şarta uyan herhangi bir kayıt var mı diye kontrol eder
    //  o => o.orderCod=124
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    // Verilen ID'ye sahip kayıt var mı diye kontrol eder
    public Task<bool> AnyAsync(TId id);
}