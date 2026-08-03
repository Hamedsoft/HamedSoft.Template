namespace HamedSoft.Template.Application.Features.Commands.Auth.Login;

public sealed record LoginResult(
    Guid UserId,
    string UserName,
    string DisplayName,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions);