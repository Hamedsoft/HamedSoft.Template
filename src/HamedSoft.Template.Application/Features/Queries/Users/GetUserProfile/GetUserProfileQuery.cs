using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserProfile;

public sealed record GetUserProfileQuery(
    Guid UserId)
    : IQuery<Result<UserProfileDto>>;