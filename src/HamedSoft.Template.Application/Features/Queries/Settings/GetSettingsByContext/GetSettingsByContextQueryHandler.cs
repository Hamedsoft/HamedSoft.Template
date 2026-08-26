using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Settings.GetSettingsByContext;

/// <summary>
/// Handles retrieval of settings by their application context.
/// </summary>
public sealed class GetSettingsByContextQueryHandler
    : IQueryHandler<
        GetSettingsByContextQuery,
        Result<IReadOnlyCollection<SettingDto>>>
{
    private readonly ISettingReadRepository _settingReadRepository;

    public GetSettingsByContextQueryHandler(
        ISettingReadRepository settingReadRepository)
    {
        _settingReadRepository = settingReadRepository;
    }

    public async Task<Result<IReadOnlyCollection<SettingDto>>> Handle(GetSettingsByContextQuery request, CancellationToken cancellationToken)
    {
        var settings = await _settingReadRepository.GetByContextAsync(request.Module, request.Feature, request.Category, cancellationToken);
        return Result<IReadOnlyCollection<SettingDto>>.Success(settings);
    }
}