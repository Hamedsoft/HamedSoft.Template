using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Login;

public sealed record LoginCommand(string UserName, string Password) : ICommand<Result<LoginResult>>;