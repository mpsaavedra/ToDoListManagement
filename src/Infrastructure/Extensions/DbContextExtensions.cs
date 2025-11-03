using Bootler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUser = Bootler.Domain.Entities.User;

namespace Bootler.Infrastructure.Extensions;

public static class DbContextExtensions
{
    public static async Task<int> SaveEntitiesChangesAsync(this AppDbContext context,
        long userId = 0, CancellationToken cancellationToken = default)
    { 
        cancellationToken.ThrowIfCancellationRequested();
        var entries = context.ChangeTracker.Entries().Where(x => x.Entity is Entity);
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        var added = entityEntries.Where(e => e.State == EntityState.Added);
        var modified = entityEntries.Where(e => e.State == EntityState.Modified);

        if (added.Any()) context.ProcessAddedEntities(userId, added);
        if (modified.Any()) context.ProcessModifiedEntities(userId, modified);

        var result = await context.SaveChangesAsync(cancellationToken);

        // dispatch events
        var domainEntities = context.ChangeTracker.Entries().Where(x =>
                                x.Entity is Entity &&
                                ((Entity)x.Entity).DomainEvents.Any())
                                .Select(x => (Entity)x.Entity);
        var domainEvents = domainEntities
                                .SelectMany(x => x.DomainEvents)
                                .ToList();
        // if (context.Dispatcher != null && domainEvents.Any())
        //     await context.Dispatcher?.DispatchEventAsync(domainEvents)!;

        domainEntities.ToList().ForEach(entity => ((Entity)entity).ClearEvents());

        return result;
    }

    private static void ProcessAddedEntities(this AppDbContext context, long? userId,
        IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ((Entity)entry.Entity).CreatedAt = DateTime.UtcNow;
                    ((Entity)entry.Entity).CreatedBy = userId.HasValue ? userId.Value : 1;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Modified:
                default:
                    throw new ApplicationException($"Entry {entry} is not Added state");
            }
        }
    }

    private static void ProcessModifiedEntities(this AppDbContext context, long? userId,
        IEnumerable<EntityEntry> entries)
    {
        var entityEntries = entries as EntityEntry[] ?? entries.ToArray();
        foreach (var entry in entityEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    ((Entity)entry.Entity).LastUpdatedAt = DateTime.UtcNow;
                    ((Entity)entry.Entity).LastUpdatedBy = userId.HasValue ? userId.Value : 1;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                case EntityState.Added:
                default:
                    throw new ApplicationException($"Entry {entry} is not Modified state");
            }
        }
    }

    /// <summary>
    /// Execute provided operation in a transactional and resilient environment 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="operation"></param>
    /// <param name="verifySucceeded"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> ExecuteAsync<TResult>(this AppDbContext ctx,
        Func<Task<TResult>> operation, Task<bool>? verifySucceeded = null, int userId = 0,
        CancellationToken cancellationToken = default)
    {
        verifySucceeded ??= new Task<bool>(() => true);
        var executionStrategy = ctx.Database.CreateExecutionStrategy();
        AppUser user;
        //if (!ctx.Users.Any() || ctx.CurrentUserId == null || ctx.CurrentUserId <= 0)
        //{
        //    await CreateAdminUser(ctx);
        //    user = await ctx.Users.FirstAsync(x => x.UserName == "admin");
        //}
        //else
        //    user = await ctx.Users.FirstAsync(x => x.Id == ctx.CurrentUserId);
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

    private static async Task<long> CreateAdminUser(AppDbContext ctx)
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
}