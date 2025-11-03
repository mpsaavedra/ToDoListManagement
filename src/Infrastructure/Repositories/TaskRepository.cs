using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTask = Bootler.Domain.Entities.Task;

namespace Bootler.Infrastructure.Repositories;


public interface ITaskRepository : IRepository<AppTask>
{
}

public class TaskRepository : Repository<AppTask>, ITaskRepository
{
    public TaskRepository(AppDbContext ctx, ICurrentUserService currentUser) : base(ctx, currentUser)
    {
    }
}
