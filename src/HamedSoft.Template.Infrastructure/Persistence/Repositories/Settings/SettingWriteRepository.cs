using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Domain.Settings;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Persistence.Repositories.Settings;

internal sealed class SettingWriteRepository : ISettingWriteRepository
{
    private readonly ApplicationDbContext _context;

    public SettingWriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Setting setting,
        CancellationToken cancellationToken = default)
    {
        await _context.Settings.AddAsync(
            setting,
            cancellationToken);
    }

    public async Task UpdateAsync(
        Setting setting,
        CancellationToken cancellationToken = default)
    {
        _context.Settings.Update(setting);

        await Task.CompletedTask;
    }

    public async Task RemoveAsync(
        Setting setting,
        CancellationToken cancellationToken = default)
    {
        _context.Settings.Remove(setting);

        await Task.CompletedTask;
    }
    public async Task<bool> SetValueAsync(
        string key,
        string value,
        CancellationToken cancellationToken = default)
    {
        var setting = await _context.Settings
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);

        if (setting is null)
            return false;

        setting.ChangeValue(value);

        return true;
    }

    public async Task<bool> RemoveByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        var setting = await _context.Settings
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);

        if (setting is null)
            return false;

        _context.Settings.Remove(setting);

        return true;
    }
    public async Task<Setting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.Settings.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
    }
}