using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    //it must be named InvokeAsync for ASP.NET Core to recognize it as middleware.
    //frameowork will call this method
    //ASP.NET Core uses built-in dependency injection and Microsoft.Extensions.Logging framework.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);    
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment() ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
