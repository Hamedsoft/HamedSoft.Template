using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Register;

public sealed record RegisterCommand(string UserName, string Password, string FirstName, string LastName) : ICommand<Result<RegisterResult>>;