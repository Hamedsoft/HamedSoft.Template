namespace HamedSoft.Template.Application.Contracts.Common;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}