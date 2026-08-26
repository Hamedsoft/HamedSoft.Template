using System.Linq;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Settings;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence.Repositories.Settings;

internal sealed class SettingReadRepository : ISettingReadRepository
{
    private readonly ApplicationDbContext _context;

    public SettingReadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SettingDto?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await _context.Settings
            .AsNoTracking()
            .Where(x => x.Key == key)
            .Select(x => new SettingDto(
                x.Id,
                x.Key,
                x.Module,
                x.Feature,
                x.Category,
                x.Value,
                x.DefaultValue,
                (int)x.ValueType,
                x.IsRequired,
                x.IsSensitive,
                x.IsSecret,
                x.Description))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<SettingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Settings
            .AsNoTracking()
            .OrderBy(x => x.Module)
            .ThenBy(x => x.Feature)
            .ThenBy(x => x.Category)
            .ThenBy(x => x.Key)
            .Select(x => new SettingDto(
                x.Id,
                x.Key,
                x.Module,
                x.Feature,
                x.Category,
                x.Value,
                x.DefaultValue,
                (int)x.ValueType,
                x.IsRequired,
                x.IsSensitive,
                x.IsSecret,
                x.Description))
            .ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyCollection<SettingDto>> GetByContextAsync(string? module, string? feature, string? category, CancellationToken cancellationToken = default)
    {
        var result = 
        await _context.Settings
            .AsNoTracking()
            .OrderBy(x => x.Module)
            .ThenBy(x => x.Feature)
            .ThenBy(x => x.Category)
            .ThenBy(x => x.Key)
            .Where(x => 
                  (x.Module == module || module == null) && 
                  (x.Feature == feature || feature  == null) && 
                  (x.Category == category || category == null))
            .Select(x => new SettingDto(
                x.Id,
                x.Key,
                x.Module,
                x.Feature,
                x.Category,
                x.Value,
                x.DefaultValue,
                (int)x.ValueType,
                x.IsRequired,
                x.IsSensitive,
                x.IsSecret,
                x.Description))
            .ToListAsync(cancellationToken);
        return result;
    }
}