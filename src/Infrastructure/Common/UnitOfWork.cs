using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    public IDbContextFactory<AppDbContext> DbContextFactory => throw new NotImplementedException();

    public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, Task<bool>? verifySucceeded = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(Action operation, Func<bool>? verifySucceeded = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    IRepository<TEntity>? IUnitOfWork.Repository<TEntity>(bool hasCustomRepository)
    {
        throw new NotImplementedException();
    }
}
