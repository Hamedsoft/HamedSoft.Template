using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Contracts.UnitOfWork;

namespace HamedSoft.Template.Infrastructure.Initialization;

public sealed class SettingInitializer : IInitializer
{
    private readonly ISettingDefinitionProvider _definitionProvider;
    private readonly ISettingReadRepository _readRepository;
    private readonly ISettingWriteRepository _writeRepository;
    private readonly IApplicationUnitOfWork _unitOfWork;

    public SettingInitializer(
        ISettingDefinitionProvider definitionProvider,
        ISettingReadRepository readRepository,
        ISettingWriteRepository writeRepository, IApplicationUnitOfWork unitOfWork)
    {
        _definitionProvider = definitionProvider;
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        var definitions = _definitionProvider.GetDefinitions();

        foreach (var definition in definitions)
        {
            var existing = await _readRepository.GetByKeyAsync(
                definition.Key,
                cancellationToken);

            if (existing is not null)
                continue;

            var setting = Domain.Settings.Setting.Create(
                definition.Key,
                definition.Module,
                definition.Feature,
                definition.Category,
                definition.DefaultValue ?? string.Empty,
                definition.ValueType,
                definition.DefaultValue,
                definition.IsRequired,
                definition.IsSensitive,
                definition.IsSecret,
                definition.Description);

            await _writeRepository.AddAsync(
                setting,
                cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}