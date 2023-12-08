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
        }
        else if (exeptionType == typeof(NotFoundException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotFound;
            errors = ex.Errors;
        }
        else if (exeptionType == typeof(BadRequestException))
        {
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
            errors = ex.Errors;
        }
        else if (exeptionType == typeof(InternalIdentityServerException))
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
            errors = ex.Errors;
        }
        else
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
        }

        var response = new ExceptionOccuredReadDTO(message, errors, status);
        var exeptionResult = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(exeptionResult);
    }
}
