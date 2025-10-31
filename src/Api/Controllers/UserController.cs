using System.Net;
using System.Reflection.Metadata.Ecma335;
using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses.Users;
using Bootler.Infrastructure.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bootler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("SignIn")]
    public async Task<ActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        var result = await _mediator.Send(new SignInCommand(request));
        return ToWithStatusCode(result,
            result.Success ?
                $"User {request.UserName} sign in success!" :
                $"Couldn't sign in user {request.UserName}s",
            result.Success 
             ? HttpStatusCode.OK
             : HttpStatusCode.Unauthorized);
    }

    [HttpPost("SignOut")]
    public async Task<IActionResult> SignOutAsync()
    {
        var result = await _mediator.Send(new SignOutCommand());
        return ToWithStatusCode(result,
            result.Success 
                ? "User signout success!"
                : "Couldn't sign in user",
            result.Success 
                ?  HttpStatusCode.OK
                : HttpStatusCode.InternalServerError
        );
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
    {
        var result = await _mediator.Send(new SignUpCommand(request));
        return ToWithStatusCode(result,
            result.Success && result.Data.Id > 0
                ? $"User  {request.UserName} sign in success!"
                : $"Couldn't sign user {request.UserName}",
            !result.Success
                ? HttpStatusCode.OK
                : HttpStatusCode.InternalServerError);
    }
}