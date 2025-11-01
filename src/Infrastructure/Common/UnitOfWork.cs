using Bootler.Domain.Entities;
using Bootler.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppRole = Bootler.Domain.Entities.Role;
using AppTask = Bootler.Domain.Entities.Task;
using AppUser = Bootler.Domain.Entities.User;
using AppUserTask = Bootler.Domain.Entities.UserTask;

namespace Bootler.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _provider;
    private readonly ICurrentUserService _currentUser;
    public const int SYSTEM_USER_ID = 1;

    protected bool disposed = false;
    protected Dictionary<Type, object> repositories = new();

    public IDbContextFactory<AppDbContext> DbContextFactory { get; }

    public UnitOfWork(IDbContextFactory<AppDbContext> dbContextFactory, IServiceProvider serviceProvider,
        ICurrentUserService currentUserService)
    {
        DbContextFactory = dbContextFactory;
        _provider = serviceProvider;
        _currentUser = currentUserService;
    }

    private async Task<long> CreateAdminUser(AppDbContext ctx)
    {
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
        var ctx = await DbContextFactory.CreateDbContextAsync();
        var executionStrategy = ctx.Database.CreateExecutionStrategy();
        AppUser user;
        if (!ctx.Users.Any() || _currentUser.GetUserId() == null || _currentUser.GetUserId() <= 0)
        {
            await CreateAdminUser(ctx);
            user = await ctx.Users.FirstAsync(x => x.UserName == "admin");
        }
        else
            user = await ctx.Users.FirstAsync(x => x.Id == _currentUser.GetUserId());
        //var user = await ctx.Users.FirstAsync(x => x.UserName == currentUserName);
        if (ctx.Database.ProviderName == null || ctx.Database.ProviderName.Contains("InMemory"))
        {
            // InMemory does no support transactions
            // this should be in tests only
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var result = operation.Invoke();
                //if (ctx is AppDbContext context)
                //    await context.SaveEntitiesChangesAsync(user != null ? user.Id : 0, cancellationToken);
                //else
                await ctx.SaveChangesAsync(cancellationToken);
                return await result;
            });
        }

        return await executionStrategy.ExecuteAsync(async ct =>
        {
            await using var transaction = await ctx.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await operation.Invoke();

                //if (ctx is AppDbContext context)
                //    await context.SaveEntitiesChangesAsync(user.Id, cancellationToken);
                //else
                await ctx.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }, cancellationToken);
    }

    public System.Threading.Tasks.Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null, CancellationToken cancellationToken = default) =>
            System.Threading.Tasks.Task.Run(async () =>
            {
                var ctx = await DbContextFactory.CreateDbContextAsync();
                verifySucceeded ??= (() => false);
                var executionStrategy = ctx.Database.CreateExecutionStrategy();
                User user;
                if (!ctx.Users.Any() || _currentUser.GetUserId() == null || _currentUser.GetUserId() < 0)
                {
                    await CreateAdminUser(ctx);
                    user = await ctx.Users.FirstAsync(x => x.UserName == "admin");
                }
                else
                    user = await ctx.Users.FirstAsync(x => x.Id == _currentUser.GetUserId());
                if (ctx.Database.ProviderName == null || ctx.Database.ProviderName.Contains("InMemory"))
                {
                    // InMemory does no support transactions
                    // this should be in tests only
                    _ = executionStrategy.Execute(async () =>
                    {
                        operation.Invoke();
                        //if (ctx is AppDbContext context)
                        //    await context.SaveEntitiesChangesAsync(user.Id, cancellationToken);
                        //else
                        await ctx.SaveChangesAsync(cancellationToken);
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
                        await ctx.SaveChangesAsync(cancellationToken);
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
            repositories[type] = new Repository<TEntity>((IDbContextFactory<AppDbContext>)DbContextFactory, _currentUser);
        }


        return (IRepository<TEntity>?)repositories[type];
    }
}