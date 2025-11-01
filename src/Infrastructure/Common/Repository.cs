using Bootler.Data;
using Bootler.Infrastructure.Services;
using Bootler.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
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
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly ICurrentUserService _currentUser;

    /// <summary>
    /// Initializes a new instance of the repository.
    /// </summary>
    /// <param name="factory">DB context factory for creating contexts.</param>
    /// <param name="currentUser">Service to obtain the current user information.</param>
    public Repository(IDbContextFactory<AppDbContext> factory, ICurrentUserService currentUser)
    {
        _factory = factory;
        _currentUser = currentUser;
    }

    public async Task<bool> AnyASync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        if (predicate != null) 
            return await ctx.Set<TEntity>().AnyAsync(predicate);
        return await ctx.Set<TEntity>().AnyAsync();
    }

    public async Task<long?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var id = await ctx.Set<TEntity>().AddAsync(entity);
        var saved = await ctx.SaveEntitiesChangesAsync(0, cancellationToken);
        if (id.Entity.Id <= 0)
            throw new ApplicationException($"Could not create new entity {nameof(TEntity)}");
        return id.Entity.Id;
    }

    public Task<bool> DeleteAsync(long id, bool softDelete = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>>? predicate = null, bool softDelete = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBY = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool includeSoftDeleted = false, bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool includeSoftDeleted = false, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> UpdateAsync(long id, TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}