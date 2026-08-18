namespace HamedSoft.Template.Web.ViewModels.Error;

public sealed class ErrorViewModel
{
    public int StatusCode { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string? CorrelationId { get; init; }

    public string? TraceId { get; init; }

    public bool ShowCorrelationId => !string.IsNullOrWhiteSpace(CorrelationId);

    public bool ShowTraceId => !string.IsNullOrWhiteSpace(TraceId);
}