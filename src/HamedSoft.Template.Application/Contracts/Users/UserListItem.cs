namespace HamedSoft.Template.Application.Contracts.Users;

public sealed record UserListItem(
    Guid UserId,
    string UserName,
    string DisplayName,
    UserProfileDto UserProfile,
    IReadOnlyCollection<string> Roles);