namespace HamedSoft.Template.Application.Common.Paging;

public sealed record PageRequest
{
    public const int DefaultPageSize = 20;

    public const int MaxPageSize = 100;

    public int PageNumber { get; }

    public int PageSize { get; }

    public int Skip =>
        (NormalizedPageNumber - 1) * NormalizedPageSize;

    public int NormalizedPageNumber =>
        PageNumber < 1
            ? 1
            : PageNumber;

    public int NormalizedPageSize =>
        PageSize switch
        {
            <= 0 => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _ => PageSize
        };

    public PageRequest(
        int pageNumber = 1,
        int pageSize = DefaultPageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}