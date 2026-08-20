using System.Net;
using System.Text.Json;
using AutoService.Business.Exceptions;

namespace AutoService.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleException(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (BusinessException ex)
        {
            await HandleException(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (CustomValidationException ex)
        {
            await HandleException(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception)
        {
            await HandleException(
                context,
                HttpStatusCode.InternalServerError,
                "Beklenmeyen bir hata oluştu.");
        }
    }

    private static async Task HandleException(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Error = message
        });

        await context.Response.WriteAsync(result);
    }
}