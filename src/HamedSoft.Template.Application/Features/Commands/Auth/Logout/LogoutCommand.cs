using HamedSoft.Template.Application.Messaging;
using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Application.Features.Commands.Auth.Logout;

public sealed record LogoutCommand() : ICommand<Result>;