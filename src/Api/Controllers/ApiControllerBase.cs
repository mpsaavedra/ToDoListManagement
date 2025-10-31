using System.Net;
using Bootler.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Bootler.Api.Controllers;

public class ApiControllerBase : ControllerBase
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ToWithStatusCode(object? data, string? message = null,
        HttpStatusCode statusCode = HttpStatusCode.OK)

    {
        message = message ?? string.Empty;
        var response = new BaseResponse { Message = message, StatusCode = statusCode, Data = data };
        return StatusCode((int)statusCode, response);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ToWithStatusCode(object? data,
        HttpStatusCode statusCode = HttpStatusCode.OK)

    {
        var response = new BaseResponse { StatusCode = statusCode, Data = data };
        return StatusCode((int)statusCode, response);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public ObjectResult ToStatusCode<T>(T? result, HttpStatusCode statusCode = HttpStatusCode.OK) 
        where T : BaseResponse
    {
        return StatusCode((int)result!.StatusCode, result);
    }
}
