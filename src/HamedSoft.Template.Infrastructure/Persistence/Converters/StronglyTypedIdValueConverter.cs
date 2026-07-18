using System.Linq.Expressions;
using HamedSoft.Template.Domain.SeedWork;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HamedSoft.Template.Infrastructure.Persistence.Converters;

public sealed class StronglyTypedIdValueConverter<TStronglyTypedId, TValue>
    : ValueConverter<TStronglyTypedId, TValue>
    where TStronglyTypedId : StronglyTypedId<TValue, TStronglyTypedId>
    where TValue : notnull
{
    public StronglyTypedIdValueConverter()
        : base(ToProviderExpression(), FromProviderExpression())
    {
    }

    private static Expression<Func<TStronglyTypedId, TValue>> ToProviderExpression()
    {
        return id => id.Value;
    }

    private static Expression<Func<TValue, TStronglyTypedId>> FromProviderExpression()
    {
        var createMethod = typeof(TStronglyTypedId).GetMethod(
            "Create",
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static,
            [typeof(TValue)]);

        if (createMethod is null)
        {
            throw new InvalidOperationException(
                $"{typeof(TStronglyTypedId).Name} must expose a public static Create({typeof(TValue).Name}) method.");
        }

        var parameter = Expression.Parameter(typeof(TValue), "value");

        var body = Expression.Call(createMethod, parameter);

        return Expression.Lambda<Func<TValue, TStronglyTypedId>>(
            body,
            parameter);
    }
}