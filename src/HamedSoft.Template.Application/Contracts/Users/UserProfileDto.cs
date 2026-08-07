namespace HamedSoft.Template.Application.Contracts.Users;

public sealed record UserProfileDto(
    Guid UserId,
    string UserName,
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber);