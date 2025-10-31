using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Tasks;

public record FindTasksRequest(IEnumerable<string> Filters, string? OrderBy = null, int PageIndex = 0, int PageSize = 50);
