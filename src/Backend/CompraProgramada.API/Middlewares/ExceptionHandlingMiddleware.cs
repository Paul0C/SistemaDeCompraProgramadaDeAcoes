using CompraProgramada.Application.SharedContext.Behaviors;
using CompraProgramada.Application.SharedContext.Results;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(Result.Failure(
                new Error(context.Response.StatusCode.ToString(), ex.Message)));
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(Result.Failure(
                new Error(context.Response.StatusCode.ToString(), string.Join(";", ex.Errors.Select(x => x.ErrorMessage)))));
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(Result.Failure(
                new Error(context.Response.StatusCode.ToString(), ex.Message)));
        }
    }
}