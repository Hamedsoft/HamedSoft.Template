namespace HamedSoft.Template.Application.Contracts.Authentication;

public sealed record AuthenticatedUser(
    Guid UserId,
    string UserName,
    string DisplayName,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);