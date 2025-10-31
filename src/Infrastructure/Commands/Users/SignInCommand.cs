using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Commands.Users;

public record SignInCommand(SignInRequest SignInRequest)  : IRequest<UserSignInResponse>;
