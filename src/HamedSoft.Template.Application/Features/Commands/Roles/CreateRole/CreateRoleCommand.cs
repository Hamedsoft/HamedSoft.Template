using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Features.Commands.Roles.CreateRole;

public sealed record CreateRoleCommand(
    string RoleName) : IRequest<Result<Guid>>;