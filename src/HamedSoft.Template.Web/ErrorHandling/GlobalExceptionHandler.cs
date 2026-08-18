using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HamedSoft.Template.Web.ErrorHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var correlationId =
            httpContext.Items[CorrelationIdHeader]?.ToString()
            ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Unhandled exception.");

        if (IsApiRequest(httpContext) ||
            IsAjaxRequest(httpContext))
        {
            await WriteProblemDetailsAsync(
                httpContext,
                correlationId,
                cancellationToken);

            return true;
        }

        // MVC request:
        // اجازه می‌دهیم ExceptionHandlerMiddleware
        // درخواست را به ErrorController بازاجرا کند.
        return false;
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext httpContext,
        string correlationId,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "خطای داخلی سرور",
            Detail = "خطایی در پردازش درخواست رخ داد.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["correlationId"] =
            correlationId;

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);
    }

    private static bool IsApiRequest(
        HttpContext context)
    {
        return context.Request.Path
            .StartsWithSegments("/api");
    }

    private static bool IsAjaxRequest(
        HttpContext context)
    {
        return string.Equals(
            context.Request.Headers["X-Requested-With"],
            "XMLHttpRequest",
            StringComparison.OrdinalIgnoreCase);
    }
}