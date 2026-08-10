namespace HamedSoft.Template.Application.Contracts.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    Guid? UserId { get; }

    string? UserName { get; }

    string? DisplayName { get; }

    IReadOnlyCollection<string> Roles { get; }

    bool IsInRole(string role);
}