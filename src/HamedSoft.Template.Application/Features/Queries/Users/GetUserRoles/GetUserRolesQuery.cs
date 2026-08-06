using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUserRoles;

public sealed record GetUserRolesQuery(
    Guid UserId)
    : IQuery<Result<UserRolesDto>>;