namespace HamedSoft.Template.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? UserName { get; }

    bool IsAuthenticated { get; }
}