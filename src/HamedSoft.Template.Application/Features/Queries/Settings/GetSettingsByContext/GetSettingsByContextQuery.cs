using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Settings.GetSettingsByContext;

/// <summary>
/// Retrieves settings belonging to a specific module, feature and category.
/// </summary>
public sealed record GetSettingsByContextQuery(string? Module, string? Feature, string? Category) : IQuery<Result<IReadOnlyCollection<SettingDto>>>;