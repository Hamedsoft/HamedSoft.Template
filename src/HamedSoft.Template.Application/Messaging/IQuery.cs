using MediatR;

namespace HamedSoft.Template.Application.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}