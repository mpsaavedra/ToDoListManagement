using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Services;

public class UserService : IUserService
{
    public Task<bool> IsAdminAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public Task<SignInResponse> SignInAsync(SignInRequest userSignInRequest)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SignOutAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SignUpResponse> SignUpAsync(SignUpRequest userSignInRequest)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyPasswordAsync(string UserName, string Password)
    {
        throw new NotImplementedException();
    }
}
