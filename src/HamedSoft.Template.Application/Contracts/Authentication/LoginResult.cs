namespace HamedSoft.Template.Application.Contracts.Authentication;

public sealed record LoginResult(Guid UserId, string UserName, IReadOnlyCollection<string> Roles);