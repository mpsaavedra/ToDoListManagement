using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly Claim? _userClaim = null;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var first = httpContextAccessor.HttpContext?.User.FindFirst("UserId");
        if (first != null) _userClaim = first;
        _userClaim = httpContextAccessor.HttpContext?.User.FindFirst(first is null ? "sub" : ClaimTypes.NameIdentifier);
    }

    public long? GetUserId()
    {
        if(_userClaim != null && long.TryParse(_userClaim.Value, out var userId)) 
            return userId;
        var first = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
        return null;
    }
}
