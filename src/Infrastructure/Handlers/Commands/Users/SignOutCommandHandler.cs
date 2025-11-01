using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using Bootler.Infrastructure.Commands.Users;
using Bootler.Infrastructure.Services;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Infrastructure.Handlers.Commands.Users;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, BaseResponse>
{
    private readonly IUserService _userService;

    public SignOutCommandHandler(IUserService userService)
    {
        this._userService = userService;
    }
    public async Task<BaseResponse> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {

        try
        {
            Log.Debug($"SignOut current user");
            var result = await _userService.SignOutAsync();
            var msg = result ? $"Could not SignOut current user" : "User SignedOut successfully";
            Log.Debug(msg);
            return BaseResponse.Succeed("User successfully Signed Out");
        }
        catch (Exception ex)
        {
            Log.Error("Error while signing in user {UserName}", request.Input.UserName);
            return BaseResponse.Fail<BaseResponse<SignInResponse>>("Error while signing in user {UserName}");
        }
    }
}
