using Bootler.Data;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Bootler.Infrastructure.Common;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    Task<long?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBY = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int pageIndex = 0, int pageSize = 50, bool includeSoftDeleted = false,
        bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null, bool includeSoftDeleted = false,
        bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<TEntity?> UpdateAsync(long id, TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, bool softDelete = true, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>>? predicate = null, bool softDelete = true,
            CancellationToken cancellationToken = default);
    Task<bool> AnyASync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}
