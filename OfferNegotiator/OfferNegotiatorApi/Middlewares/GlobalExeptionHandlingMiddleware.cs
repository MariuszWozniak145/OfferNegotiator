using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorLogic.DTOs.Exception;
using System.Net;
using System.Text.Json;

namespace OfferNegotiatorApi.Middlewares;

public class GlobalExeptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExeptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException ex)
        {
            await HandleExeptionAsync(context, ex);
        }
    }
    private static Task HandleExeptionAsync(HttpContext context, BaseException ex)
    {
        HttpStatusCode status;
        string stackTrace;
        string message;
        List<string> errors = new();

        var exeptionType = ex.GetType();

        if (exeptionType == typeof(WrongCredentialsException))
        {
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
            errors = ex.Errors;
            stackTrace = ex.StackTrace;
        }
        else if (exeptionType == typeof(UnauthorizedAccessException))
        {
            message = ex.Message;
            status = HttpStatusCode.Unauthorized;
            errors = ex.Errors;
            stackTrace = ex.StackTrace;
        }
        else
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
            stackTrace = ex.StackTrace;
        }

        var response = new ExceptionOccuredReadDTO(message, errors, status);
        var exeptionResult = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(exeptionResult);
    }
}
