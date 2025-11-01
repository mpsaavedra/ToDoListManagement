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

public class SignInCommandHandler : IRequestHandler<SignInCommand, BaseResponse<SignInResponse>>
{
    private readonly IUserService _userService;

    public SignInCommandHandler(IUserService userService)
    {
        this._userService = userService;
    }

    public async Task<BaseResponse<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Debug($"Signing in user {request.Input.UserName}");
            var result = await _userService.SignInAsync(request.Input);
            var msg = result.Token!.IsNullEmptyOrWhiteSpace() ? $"Could not SignIn user {request.Input.UserName}" : "User Signed In successfully";
            Log.Debug(msg);
            return new BaseResponse<SignInResponse>
            {
                Data = result,
            };
        }
        catch (Exception ex)
        {
            Log.Error("Error while signing in user {UserName}", request.Input.UserName);
            return BaseResponse.Fail<BaseResponse<SignInResponse>>("Error while signing in user {UserName}");
        }
    }
}
