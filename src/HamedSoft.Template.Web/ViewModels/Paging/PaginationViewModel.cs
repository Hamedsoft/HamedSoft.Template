namespace HamedSoft.Template.Web.ViewModels.Paging;

public sealed record PaginationViewModel(
    int PageNumber,
    int PageSize,
    int TotalPages,
    string Action,
    string Controller,
    IReadOnlyDictionary<string, string?>? RouteValues = null)
{
    public bool HasPreviousPage =>
        PageNumber > 1;

    public bool HasNextPage =>
        PageNumber < TotalPages;
}