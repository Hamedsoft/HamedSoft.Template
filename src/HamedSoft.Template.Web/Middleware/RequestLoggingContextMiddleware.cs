using HamedSoft.Template.Application.Contracts.Security;
using Serilog;
using Serilog.Context;

namespace HamedSoft.Template.Web.Middleware;

public sealed class RequestLoggingContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingContextMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IDiagnosticContext diagnosticContext,
        ICurrentUser currentUser)
    {
        diagnosticContext.Set(
            "RequestPath",
            context.Request.Path.Value);

        diagnosticContext.Set(
            "HttpMethod",
            context.Request.Method);

        if (currentUser.UserId.HasValue)
        {
            diagnosticContext.Set(
                "UserId",
                currentUser.UserId.Value);
        }

        await _next(context);
    }
}