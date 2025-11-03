using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using Bootler.Contracts.Responses.Users;
using MediatR;

namespace Bootler.Infrastructure.Queries.Tasks;

public record GetAllTasksQuery(GetAllTasksRequest Input) : IRequest<BaseResponse<GetAllTasksResponse>>;