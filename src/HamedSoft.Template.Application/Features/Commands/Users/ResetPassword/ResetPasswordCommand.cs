using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Users.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string NewPassword)
    : ICommand<Result>;