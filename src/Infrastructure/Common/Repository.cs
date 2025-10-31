using Bootler.Data;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Common;


public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    public Task<bool> AnyASync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long?> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>>? predicate = null, bool softDelete = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<T>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBY = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0, int pageSize = 50, bool includeSoftDeleted = false, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, bool includeSoftDeleted = false, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
