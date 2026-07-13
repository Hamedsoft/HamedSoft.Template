using MediatR;

namespace HamedSoft.Template.Application.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}