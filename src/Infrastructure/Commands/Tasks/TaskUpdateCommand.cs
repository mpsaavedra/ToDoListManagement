using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using MediatR;

namespace Bootler.Infrastructure.Commands.Tasks;

public record TaskUpdateCommand(TaskUpdateRequest Input) : IRequest<BaseResponse>;