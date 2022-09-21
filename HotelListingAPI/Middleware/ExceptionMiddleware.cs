using HotelListing.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware>  logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            ErrorType = "Failure",
            ErrorMessage = ex.Message
        };

        switch (ex)
        {
            case ArgumentException _:
                statusCode = HttpStatusCode.BadRequest;
                errorDetails.ErrorType = "BadRequest";
                break;
            case NotFoundException _:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "NotFound";
                break;
            case UnauthorizedAccessException _:
                statusCode = HttpStatusCode.Unauthorized;
                errorDetails.ErrorType = "Unauthorized";
                break;
            default:
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        return httpContext.Response.WriteAsync(errorDetails.ToString());
    }

   
}

public class ErrorDetails
{
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}