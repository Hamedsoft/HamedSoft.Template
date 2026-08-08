namespace HamedSoft.Template.Application.Contracts.Security;

public interface ICurrentUser
{
    Guid? UserId { get; }

    bool IsAuthenticated { get; }
}