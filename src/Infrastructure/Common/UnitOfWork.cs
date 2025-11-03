using Bootler.Domain.Entities;
using Bootler.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bootler.Infrastructure.Extensions;
using AppRole = Bootler.Domain.Entities.Role;
using AppTask = Bootler.Domain.Entities.Task;
using AppUser = Bootler.Domain.Entities.User;
using AppUserTask = Bootler.Domain.Entities.UserTask;

namespace Bootler.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _provider;
    private readonly ICurrentUserService _currentUser;
    private readonly AppDbContext _ctx;
    public const int SYSTEM_USER_ID = 1;

    protected bool disposed = false;
    protected Dictionary<Type, object> repositories = new();

    public IDbContextFactory<AppDbContext> DbContextFactory { get; }

    public UnitOfWork(AppDbContext ctx, IServiceProvider serviceProvider,
        ICurrentUserService currentUserService)
    {
        _provider = serviceProvider;
        _currentUser = currentUserService;
        _ctx = ctx;
    }

    private async Task<long> CreateAdminUser(AppDbContext ctx)
    {
        if(ctx.Users.Any(x => x.UserName == "admin"))
        {
            return ctx.Users.First(x => x.UserName == "admin").Id;
        }
        var admin = new AppUser
        {
            UserName = "admin",
            Password = "admin123.*",
        };
        var entity = await ctx.Users.AddAsync(admin);
        await ctx.SaveChangesAsync();
        return entity.Entity.Id;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, Task<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default)
    {
        verifySucceeded ??= new Task<bool>(() => true);
        // await using var ctx = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var executionStrategy = _ctx.Database.CreateExecutionStrategy();
        AppUser user;
        if (!_ctx.Users.Any() || _currentUser.GetUserId() == null || _currentUser.GetUserId() <= 0)
        {
            await CreateAdminUser(_ctx);
            user = await _ctx.Users.FirstAsync(x => x.UserName == "admin", cancellationToken);
        }
        else
            user = await _ctx.Users.FirstAsync(x => x.Id == _currentUser.GetUserId(), cancellationToken);
        //var user = await ctx.Users.FirstAsync(x => x.UserName == currentUserName);
        if (_ctx.Database.ProviderName == null || _ctx.Database.ProviderName.Contains("InMemory"))
        {
            // InMemory does no support transactions
            // this should be in tests only
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var result = operation.Invoke();
                if (_ctx is AppDbContext context)
                    await context.SaveEntitiesChangesAsync(user != null ? user.Id : 0, cancellationToken);
                else
                    await _ctx.SaveChangesAsync(cancellationToken);
                return await result;
            });
        }

        return await executionStrategy.ExecuteAsync(async ct =>
        {
            await using var transaction = await _ctx.Database.BeginTransactionAsync(ct);
            try
            {
                var result = await operation.Invoke();

                // ReSharper disable once ConvertTypeCheckPatternToNullCheck
                if (_ctx is AppDbContext context)
                    await context.SaveEntitiesChangesAsync(user.Id, ct);
                else
                    await _ctx.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }, cancellationToken);
    }

    public System.Threading.Tasks.Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null, CancellationToken cancellationToken = default) =>
            System.Threading.Tasks.Task.Run(async () =>
            {
                // await using var ctx = await DbContextFactory.CreateDbContextAsync();
                verifySucceeded ??= (() => false);
                var executionStrategy = _ctx.Database.CreateExecutionStrategy();
                User user;
                if (!_ctx.Users.Any() || _currentUser.GetUserId() == null || _currentUser.GetUserId() < 0)
                {
                    await CreateAdminUser(_ctx);
                    user = await _ctx.Users.FirstAsync(x => x.UserName == "admin");
                }
                else
                    user = await _ctx.Users.FirstAsync(x => x.Id == _currentUser.GetUserId());
                if (_ctx.Database.ProviderName == null || _ctx.Database.ProviderName.Contains("InMemory"))
                {
                    // InMemory does no support transactions
                    // this should be in tests only
                    _ = executionStrategy.Execute(async () =>
                    {
                        operation.Invoke();
                        //if (ctx is AppDbContext context)
                        //    await context.SaveEntitiesChangesAsync(user.Id, cancellationToken);
                        //else
                        await _ctx.SaveChangesAsync(cancellationToken);
                    });
                }
                else
                {
                    _ = executionStrategy.ExecuteInTransaction(async () =>
                    {
                        operation.Invoke();
                        //if (ctx is AppDbContext context)
                        //    await context.SaveEntitiesChangesAsync(user.Id, cancellationToken);
                        //else
                        await _ctx.SaveChangesAsync(cancellationToken);
                    }, verifySucceeded);
                }
            }, cancellationToken);

    IRepository<TEntity>? IUnitOfWork.Repository<TEntity>(bool hasCustomRepository)
    {
        if (hasCustomRepository)
        {
            if (_provider.GetService(typeof(IRepository<TEntity>))
                is IRepository<TEntity> customRepo)
            {
                return (IRepository<TEntity>?)customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!repositories.ContainsKey(type))
        {
            // save to our local cache
            repositories[type] = new Repository<TEntity>(_ctx, _currentUser);
        }


        return (IRepository<TEntity>?)repositories[type];
    }
}