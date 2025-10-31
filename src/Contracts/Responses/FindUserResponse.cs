using Bootler.Contracts.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Responses;

public record FindUserResponse(UserDetailDto? User);
