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

public interface IUserRepository : IRepository<User>
{
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IDbContextFactory<AppDbContext> factory, ICurrentUserService currentUser) : base(factory, currentUser)
    {
    }
}
