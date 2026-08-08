using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Features.Commands.Roles.UpdateRole;

public sealed record UpdateRoleCommand(
    Guid RoleId,
    string RoleName) : IRequest<Result>;