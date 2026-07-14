using MediatR;

namespace HamedSoft.Template.Application.Messaging;

/// <summary>
/// این الگو یک کاممند است که پاسخی از نوع جنریک داده شده را بر می‌گرداند
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// این الگو نشان‌دهنده یک کاممند است که هیچ پاسخی را برنمی‌گرداند
/// </summary>
public interface ICommand : IRequest
{
}