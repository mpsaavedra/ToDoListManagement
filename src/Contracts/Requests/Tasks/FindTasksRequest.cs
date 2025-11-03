using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Tasks;

public record FindTasksRequest(string[] Filters, string? OrderBy = null, int PageIndex = 1, int PageSize = 50);
