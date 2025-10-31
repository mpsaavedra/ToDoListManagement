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
    Task<UserSignInResponse> SignIn(SignInRequest userSignInRequest);
    Task<bool> SignOut();
    Task<SignUpResponse> SignUp(SignInRequest userSignInRequest);
    Task<bool> VerifyPassword(string UserName, string Password);
    Task<bool> IsAdmin(string userName);
}

public class UserService
{
}
