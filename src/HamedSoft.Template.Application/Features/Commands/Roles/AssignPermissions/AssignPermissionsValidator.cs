using FluentValidation;

namespace HamedSoft.Template.Application.Features.Commands.Roles.AssignPermissions;

public sealed class AssignPermissionsValidator
    : AbstractValidator<AssignPermissionsCommand>
{
    public AssignPermissionsValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionIds)
            .NotNull();
    }
}