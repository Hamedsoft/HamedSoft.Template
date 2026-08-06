using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Queries.Users.GetUsers;

public sealed record GetUsersQuery()
    : IQuery<Result<IReadOnlyList<UserListItem>>>;