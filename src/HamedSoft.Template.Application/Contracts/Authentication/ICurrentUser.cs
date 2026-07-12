namespace HamedSoft.Template.Application.Contracts.Authentication;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}