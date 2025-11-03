using Bootler.Contracts.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Responses.Tasks;

public record GetAllTasksResponse(PaginatedList<TaskDto> Tasks);
