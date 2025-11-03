using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Requests.Users;

namespace Bootler.Infrastructure.Commands.Users;

public record SignOutCommand(): IRequest<BaseResponse>;
