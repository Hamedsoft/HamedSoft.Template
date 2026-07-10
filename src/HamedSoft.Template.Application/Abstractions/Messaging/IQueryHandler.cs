using HamedSoft.Template.SharedKernel.Common;

namespace HamedSoft.Template.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query);
}