using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;

namespace Bootler.Infrastructure.Services;

public interface IUserService
{
    Task<SignInResponse> SignInAsync(SignInRequest userSignInRequest);
    Task<bool> SignOutAsync();
    Task<SignUpResponse> SignUpAsync(SignUpRequest userSignInRequest);
    Task<bool> VerifyPasswordAsync(string UserName, string Password);
    Task<bool> IsAdminAsync(string userName);
}
