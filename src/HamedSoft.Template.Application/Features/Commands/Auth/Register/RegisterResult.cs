namespace HamedSoft.Template.Application.Features.Commands.Auth.Register;

public sealed record RegisterResult(Guid UserId, string UserName, string DisplayName, IReadOnlyCollection<string> Roles);