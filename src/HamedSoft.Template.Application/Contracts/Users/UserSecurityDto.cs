namespace HamedSoft.Template.Application.Contracts.Users;

public sealed record UserSecurityDto(
    Guid UserId,
    string UserName,
    bool IsLocked,
    bool IsActive);