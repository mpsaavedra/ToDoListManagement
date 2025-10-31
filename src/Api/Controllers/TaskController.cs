using System.Net;
using Bootler.Contracts.Requests.Tasks;
using Bootler.Infrastructure.Commands.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bootler.Api.Controllers;

public class TaskController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
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

    [HttpPost("Update")]
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

    [HttpPost("Delete")]
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
}