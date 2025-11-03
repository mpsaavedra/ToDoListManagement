using Bootler.Contracts.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Users;

public record SignInRequest(string UserName, string Password, bool RememberMe = true);