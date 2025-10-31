using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Contracts.Responses;

public class BaseResponse<T> : BaseResponse
{
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
    public T? Data { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
}

public class BaseResponse
{
    /// <summary>
    /// creates a fail response with some message
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static T Fail<T>(string msg) where T : BaseResponse, new()
    {
        return new T
        {
            Success = false,
            Message = msg
        };
    }

    /// <summary>
    /// creates a fail response from some exception data
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static T Fail<T>(Exception e) where T : BaseResponse, new() =>
        Fail<T>(e.Message);

    /// <summary>
    /// creates a fail response with some message, it returns any previous data
    /// set in the response
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public T ToFail<T>(string msg,
                        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        where T : BaseResponse, new()
    {
        Success = false;
        Message = msg;
        Errors.Add(msg);
        StatusCode = statusCode;
        return new T
        {
            Success = false,
            Message = msg,
            Errors = Errors,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// creates a fail response from some exception data, it returns any previous data
    /// set in the response
    /// </summary>
    /// <param name="e"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public T ToFail<T>(Exception e,
                        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        where T : BaseResponse, new() =>
        ToFail<T>(e.Message, statusCode);

    /// <summary>
    /// get/set the request status
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// <see cref="HttpStatusCode"/> of the result of the operation to add some
    /// descriptive data in cases that the request execution went wrong
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// message sent from server, in some cases it could be used to confirm an operation
    /// or as a simple notification message
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// List of error messages in the execution if any
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Base response data
    /// </summary>
    public virtual object? Data { get; set; } = null;
}