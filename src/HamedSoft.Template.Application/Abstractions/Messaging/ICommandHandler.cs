using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command);
}