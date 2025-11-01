using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using MediatR;

namespace Bootler.Infrastructure.Commands.Tasks;

public record TaskCreateCommand(TaskCreateRequest Input) : IRequest<BaseResponse<TaskCreateResponse>>;