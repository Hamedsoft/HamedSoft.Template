using HamedSoft.Template.Application.Common.Paging;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence.Extensions;

public static class QueryablePagingExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PageRequest request,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .Skip(request.Skip)
            .Take(request.NormalizedPageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(
            items,
            request.NormalizedPageNumber,
            request.NormalizedPageSize,
            totalCount);
    }
}