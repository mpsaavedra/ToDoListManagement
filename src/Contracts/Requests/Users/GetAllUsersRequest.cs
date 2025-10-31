using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Users;

public record GetAllUsersRequest(int PageIndex = 0, int PageSize = 50, bool SoftDeleted= false);
