using Bootler.Data;
using Microsoft.EntityFrameworkCore;

namespace Bootler.Infrastructure.Common;

public interface IUnitOfWork
{
    /// <summary>
    /// The <see cref="IDbContextFactory{TContext}"/> used to retrieve the DbContext instance
    /// </summary>
    IDbContextFactory<AppDbContext> DbContextFactory { get; }

    /// <summary>
    /// Executes an asynchronous operation inside a resilient execution environment and returns a result.
    /// </summary>
    /// <param name="operation">The asynchronous operation to execute.</param>
    /// <param name="verifySucceeded">Optional asynchronous verifier to determine if the operation succeeded.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <typeparam name="TResult">The type of the returned result.</typeparam>
    /// <returns>The result produced by the operation.</returns>
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation,
        Task<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a synchronous action inside a resilient execution environment.
    /// </summary>
    /// <param name="operation">The synchronous action to execute.</param>
    /// <param name="verifySucceeded">Optional verifier to determine if the operation succeeded.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves a repository instance from the DI container.
    /// </summary>
    /// <param name="hasCustomRepository">If true, attempts to resolve a custom repository implementation.</param>
    /// <typeparam name="TEntity">Entity type for the repository.</typeparam>
    /// <returns>An <see cref="IRepository{TEntity}"/> instance or null if not available.</returns>
    IRepository<TEntity>? Repository<TEntity>(bool hasCustomRepository = false)
        where TEntity : class, IEntity, new();
}
