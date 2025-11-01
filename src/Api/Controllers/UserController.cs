using Bootler.Contracts.DTOs.Users;
using Bootler.Contracts.Requests.Users;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using Bootler.Infrastructure.Commands.Users;
using Bootler.Infrastructure.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;

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
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<SignInResponse>), StatusCodes.Status200OK)]
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
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
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
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<SignUpResponse>), StatusCodes.Status200OK)]
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

    [HttpGet("GetAllUsers")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<PaginatedList<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetAllUsersRequest request)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(request));
        return ToWithStatusCode(result,
            result.Success
                ? $"Retrieving  {result.Data!.Users.TotalCount} sign in success!"
                : $"Couldn't retrieving users",
            !result.Success
                ? HttpStatusCode.OK
                : HttpStatusCode.InternalServerError);
    }

    [HttpGet("FindUsers")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<PaginatedList<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FindUsersAsync([FromQuery] FindUsersRequest request)
    {
        var result = await _mediator.Send(new FindUsersQuery(request));
        return ToWithStatusCode(result,
            result.Success
                ? $"Retrieving {result.Data.Users.TotalCount} users!"
                : $"Couldn't retrieving users",
            !result.Success
                ? HttpStatusCode.OK
                : HttpStatusCode.InternalServerError);
    }
}