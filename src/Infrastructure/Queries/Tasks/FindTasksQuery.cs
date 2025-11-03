using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Queries.Tasks;

public record FindTasksQuery(FindTasksRequest Input) : IRequest<BaseResponse<FindTasksResponse>>;
