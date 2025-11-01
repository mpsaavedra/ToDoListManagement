using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Responses.Users;

public record SignUpResponse(long Id, string UserName);
