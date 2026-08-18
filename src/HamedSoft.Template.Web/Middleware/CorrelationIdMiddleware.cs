using Serilog.Context;

namespace HamedSoft.Template.Web.Middleware;

public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";
    private const int MaxLength = 64;

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetCorrelationId(context);

        context.Items[HeaderName] = correlationId;

        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(HeaderName))
            {
                context.Response.Headers.Append(
                    HeaderName,
                    correlationId);
            }

            return Task.CompletedTask;
        });

        using (LogContext.PushProperty(
            "CorrelationId",
            correlationId))
        {
            await _next(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(
                HeaderName,
                out var headerValue))
        {
            var value = headerValue.ToString().Trim();

            if (IsValidCorrelationId(value))
                return value;
        }

        return Guid.NewGuid().ToString("N");
    }

    private static bool IsValidCorrelationId(string value)
    {
        if (value.Length == 0 || value.Length > MaxLength)
            return false;

        foreach (var character in value)
        {
            if (!char.IsLetterOrDigit(character) &&
                character != '-' &&
                character != '_')
            {
                return false;
            }
        }

        return true;
    }
}