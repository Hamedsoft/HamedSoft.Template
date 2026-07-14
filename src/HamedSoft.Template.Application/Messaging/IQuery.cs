using MediatR;

namespace HamedSoft.Template.Application.Messaging;

/// <summary>
/// این الگو یک کوئری است که پاسخی از نوع جنریک داده شده را بر می‌گرداند
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}