using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler;

public static class WellKnowClaims
{
    public const string UserId = "Authorization.userId";
    public const string UserName = "Authorization.userName";
    public const string UserRole = "Authorization.Role";
}
