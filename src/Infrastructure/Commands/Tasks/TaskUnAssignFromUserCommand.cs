using Bootler.Contracts.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Commands.Tasks;

public record TaskUnAssignFromUserCommand(long TaskId, long UserId) : IRequest<BaseResponse>;
