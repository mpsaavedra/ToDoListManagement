using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Requests.Tasks;

public record TaskGetAllTasksRequest(int PageIndex = 0, int PageSize = 50, bool SoftDeleted = false);
