namespace HamedSoft.Template.Application.Common.Models;

public sealed record LookupItemDto(
    Guid Id,
    string Name,
    bool Selected);