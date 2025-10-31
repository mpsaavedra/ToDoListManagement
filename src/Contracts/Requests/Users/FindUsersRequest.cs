using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Users;

public record FindUsersRequest(IEnumerable<string> Filters, string OrderBy? = null, int PageIndex = 0, int PageSize = 50);
