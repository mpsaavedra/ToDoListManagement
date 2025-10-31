using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public long? GetUserId()
    {
        throw new NotImplementedException();
    }

    public string? GetUserName()
    {
        throw new NotImplementedException();
    }

    public bool? IsAdmin()
    {
        throw new NotImplementedException();
    }

    public bool IsAuthenticated()
    {
        throw new NotImplementedException();
    }
}
