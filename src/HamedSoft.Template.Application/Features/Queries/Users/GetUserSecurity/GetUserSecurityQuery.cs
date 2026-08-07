using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserSecurity;

public sealed record GetUserSecurityQuery(
    Guid UserId)
    : IQuery<Result<UserSecurityDto>>;