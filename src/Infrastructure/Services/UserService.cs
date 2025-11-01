using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Services;

public interface IUserService
{
    Task<SignInResponse> SignInAsync(SignInRequest userSignInRequest);
    Task<bool> SignOutAsync();
    Task<SignUpResponse> SignUpAsync(SignUpRequest userSignInRequest);
    Task<bool> VerifyPasswordAsync(string UserName, string Password);
    Task<bool> IsAdminAsync(string userName);
}

public class UserService
{
}
