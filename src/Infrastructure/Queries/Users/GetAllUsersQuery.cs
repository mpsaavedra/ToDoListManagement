using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using MediatR;

namespace Bootler.Infrastructure.Queries.Users;

public record GetAllUsersQuery(GetAllUsersRequest Input) : IRequest<BaseResponse<GetAllUsersResponse>>;