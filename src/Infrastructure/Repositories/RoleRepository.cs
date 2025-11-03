using Bootler.Domain.Entities;
using Bootler.Infrastructure.Common;
using Bootler.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Repositories;

public interface IRoleRepository: IRepository<Role>
{
}

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext ctx, ICurrentUserService currentUser) : base(ctx, currentUser)
    {
    }
}
