namespace HamedSoft.Template.Web.ViewModels.Common.Pagination;

/// <summary>
/// Represents the presentation options required to render a paginated list.
/// </summary>
public sealed class PaginationViewModel
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public bool HasPreviousPage =>
        PageNumber > 1;

    public bool HasNextPage =>
        PageNumber < TotalPages;

    public string Action { get; init; } = "Index";

    public string? Controller { get; init; }

    /// <summary>
    /// Gets the query-string parameter used by this pagination instance.
    /// </summary>
    public string PageParameterName { get; init; } = "pageNumber";

    public string PageSizeParameterName { get; init; } = "pageSize";

    public string? ActiveTab { get; init; }

    public string? Search { get; init; }

    /// <summary>
    /// Contains additional query-string parameters that must be preserved
    /// when navigating between pages.
    /// </summary>
    public IReadOnlyDictionary<string, string?> AdditionalParameters { get; init; }
        = new Dictionary<string, string?>();
}