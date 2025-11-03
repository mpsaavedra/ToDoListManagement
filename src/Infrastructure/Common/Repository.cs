using Bootler.Data;
using Bootler.Infrastructure.Services;
using Bootler.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Bootler.Infrastructure.Common;


public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IDbContextFactory<AppDbContext> _factory;
    private readonly AppDbContext _ctx;
    private readonly ICurrentUserService _currentUser;

    /// <summary>
    /// Initializes a new instance of the repository.
    /// </summary>
    /// <param name="factory">DB context factory for creating contexts.</param>
    /// <param name="currentUser">Service to obtain the current user information.</param>
    // public Repository(IDbContextFactory<AppDbContext> factory, ICurrentUserService currentUser)
    public Repository(AppDbContext ctx, ICurrentUserService currentUser)
    {
        // _factory = factory;
        _ctx = ctx;
        _currentUser = currentUser;
    }

    public async Task<bool> AnyASync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        if (predicate != null) 
            return await _ctx.Set<TEntity>().AnyAsync(predicate);
        return await _ctx.Set<TEntity>().AnyAsync();
    }

    public async Task<long?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var id = await _ctx.Set<TEntity>().AddAsync(entity);
        var userId = _currentUser.GetUserId() ?? 0;
        var saved = await _ctx.SaveEntitiesChangesAsync(userId, cancellationToken);
        if (id.Entity.Id <= 0)
            throw new ApplicationException($"Could not create new entity {nameof(TEntity)}");
        return id.Entity.Id;
    }

    public async Task<bool> DeleteAsync(long id, bool softDelete = true, 
        CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var entity = await _ctx.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            throw new ApplicationException($"Could not fount {nameof(TEntity)} with id {id}");
        if (softDelete)
        {
            entity.SoftDeleted = true;
            _ctx.Set<TEntity>().Update(entity);
        }
        else
        {
            _ctx.Set<TEntity>().Remove(entity);
        }
        var userId = _currentUser.GetUserId() ?? 0;
        var saved = await _ctx.SaveEntitiesChangesAsync(userId, cancellationToken);
        return saved > 0;
    }

    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>>? predicate = null, bool softDelete = true,
        CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var entities = _ctx.Set<TEntity>().AsQueryable();
        if (predicate != null)
            entities = entities.Where(predicate);
        foreach (var entity in entities)
        {
            if (softDelete)
            {
                entity.SoftDeleted = true;
                _ctx.Set<TEntity>().Update(entity);
            }
            else
            {
                _ctx.Set<TEntity>().Remove(entity);
            }
        }
        var userId = _currentUser.GetUserId() ?? 0;
        var saved = await _ctx.SaveEntitiesChangesAsync(userId, cancellationToken);
        return saved > 0;
    }

    public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBY = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int pageIndex = 0,
        int pageSize = 50, bool includeSoftDeleted = false, bool disableTracking = true,
        CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync();
        var query = _ctx.Set<TEntity>().AsQueryable();
        if (disableTracking)
            query = query.AsNoTracking();
        if (predicate is not null)
            query = query.Where(predicate);
        query = query.OrderBy(x => x.Id);
        if (include is not null)
            query = include(query);
        if (!includeSoftDeleted)
            query = query.Where(x => x.SoftDeleted == false);
        query = query.Skip(pageSize * pageIndex).Take(pageSize);
        return query;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        bool includeSoftDeleted = false, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var query = _ctx.Set<TEntity>().AsQueryable();
        if(disableTracking)
            query = query.AsNoTracking();
        var entity = predicate != null
            ? await query.FirstOrDefaultAsync(predicate, cancellationToken)
            : await query.FirstAsync(cancellationToken);
        return entity;
    }

    public async Task<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        var query = _ctx.Set<TEntity>().AsQueryable();
        query = query.OrderBy(x => x.Id);
        return query;
    }

    public async Task<TEntity?> UpdateAsync(long id, TEntity entity, CancellationToken cancellationToken = default)
    {
        // await using var ctx = await _factory.CreateDbContextAsync(cancellationToken);
        if (!await _ctx.Set<TEntity>().AnyAsync(x => x.Id == id))
            throw new ApplicationException($"Could not fount {nameof(TEntity)} with id {id}");
        var modifiedEntity = _ctx.Set<TEntity>().Update(entity);
        var userId = _currentUser.GetUserId() ?? 0;
        var saved = await _ctx.SaveEntitiesChangesAsync(userId, cancellationToken);
        return modifiedEntity.Entity;
    }
}