using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;

namespace Bootler.Infrastructure.Services;

public interface IUserService
{
    Task<SignInResponse> SignInAsync(SignInRequest userSignInRequest, CancellationToken cancellationToken = default);
    Task<bool> SignOutAsync(CancellationToken cancellationToken = default);
    Task<SignUpResponse> SignUpAsync(SignUpRequest userSignInRequest, CancellationToken cancellationToken = default);
    Task<bool> VerifyPasswordAsync(string UserName, string Password, CancellationToken cancellationToken = default);
    Task<bool> IsAdminAsync(string userName, CancellationToken cancellationToken = default);
}
