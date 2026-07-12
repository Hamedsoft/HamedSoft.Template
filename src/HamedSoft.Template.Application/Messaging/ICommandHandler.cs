using MediatR;

namespace HamedSoft.Template.Application.Messaging;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : IRequest<TResponse>
{
}