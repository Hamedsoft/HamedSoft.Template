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

        return true;
    }
}