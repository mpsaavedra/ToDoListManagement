using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Commands.Users;

public record SignOutCommand: IRequest<bool>;
