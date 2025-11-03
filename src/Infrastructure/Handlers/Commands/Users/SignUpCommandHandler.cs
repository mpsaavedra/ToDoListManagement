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

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, BaseResponse<SignUpResponse>>
{
    private readonly IUserService _userService;

    public SignUpCommandHandler(IUserService userService)
    {
        this._userService = userService;
    }
    public async Task<BaseResponse<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Log.Debug($"Signing in user {request.Input.UserName}");
            var result = await _userService.SignUpAsync(request.Input);
            var msg = result.Id <= 0 ? $"Could not SignUp user {request.Input.UserName}" : "User Signed Up successfully";
            Log.Debug(msg);
            return new BaseResponse<SignUpResponse>
            {
                Data = result,
            };
        }
        catch (Exception ex)
        {
            Log.Error("Error while SignUp in user {UserName}", request.Input.UserName);
            return BaseResponse.Fail<BaseResponse<SignUpResponse>>("Error while SignUp in user {UserName}");
        }
    }
}
