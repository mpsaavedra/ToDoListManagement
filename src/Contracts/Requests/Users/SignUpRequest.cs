using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Users;

public record SignUpRequest(string UserName, string Passowrd, string Role);
