using Bootler.Contracts.Requests.Tasks;
using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Tasks;
using Bootler.Contracts.Responses.Users;
using Bootler.Infrastructure.Commands.Tasks;
using Bootler.Infrastructure.Queries.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Bootler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<TaskCreateResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaskCreateCommand(request), cancellationToken);
        return ToWithStatusCode(result,
            result.Success
                ? $"Task created successfully!"
                : $"Error while creating Task",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpPut("Update")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync([FromBody] TaskUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaskUpdateCommand(request), cancellationToken);
        return ToWithStatusCode(result,
            result.Success
                ? $"Task updated successfully!"
                : $"Error while updating Task",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpDelete("Delete")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAsync([FromBody] TaskDeleteRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaskDeleteCommand(request), cancellationToken);
        return ToWithStatusCode(result,
            result.Success
                ? $"Task deleted successfully!"
                : $"Error while deleting Task",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpPost("AssignTask")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignToUserAsync([FromBody] TaskAssignToUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaskAssignToUserCommand(request), cancellationToken);
        return ToWithStatusCode(result,
            result.Success
                ? $"Task {request.TaskId} assigned to user {request.UserId} successfully!"
                : $"Error while assigning Task {request.TaskId}",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpPost("UnassignTask")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UnAssignFromUserAsync([FromBody] TaskAssignToUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new TaskAssignToUserCommand(request), cancellationToken);
        return ToWithStatusCode(result,
            result.Success
                ? $"Task {request.TaskId} assigned to user {request.UserId} successfully!"
                : $"Error while assigning Task {request.TaskId}",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpGet("GetTasks")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<PaginatedList<FindTasksResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTaskAsync([FromQuery] GetAllTasksRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTasksQuery(request));
        return ToWithStatusCode(result,
            result.Success
                ? $"Retrieving {result.Data.Tasks.TotalCount} successfully!"
                : $"Could not retrieve list of tasks",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }

    [HttpGet("FindTasks")]
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseResponse<PaginatedList<FindTasksResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FindTasksAsync([FromQuery] FindTasksRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new FindTasksQuery(request));
        return ToWithStatusCode(result,
            result.Success
                ? $"Retrieving {result.Data!.Tasks.TotalCount} successfully!"
                : $"Could not retrieve list of tasks",
            result.Success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
    }
}