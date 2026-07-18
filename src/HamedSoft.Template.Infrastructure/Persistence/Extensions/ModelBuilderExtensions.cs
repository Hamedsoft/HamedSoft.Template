using HamedSoft.Template.Domain.SeedWork;
using HamedSoft.Template.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace HamedSoft.Template.Infrastructure.Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    public static void ApplyStronglyTypedIdConversions(
        this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var propertyType = property.ClrType;

                if (!IsStronglyTypedId(propertyType))
                    continue;

                var valueType = GetValueType(propertyType);

                var converterType =
                    typeof(StronglyTypedIdValueConverter<,>)
                        .MakeGenericType(propertyType, valueType);

                var converter =
                    (ValueConverter)Activator.CreateInstance(converterType)!;

                property.SetValueConverter(converter);
            }
        }
    }

    private static bool IsStronglyTypedId(Type type)
    {
        while (type != null)
        {
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(StronglyTypedId<,>))
            {
                return true;
            }

            type = type.BaseType!;
        }

        return false;
    }

    private static Type GetValueType(Type stronglyTypedId)
    {
        while (stronglyTypedId != null)
        {
            if (stronglyTypedId.IsGenericType &&
                stronglyTypedId.GetGenericTypeDefinition() == typeof(StronglyTypedId<,>))
            {
                return stronglyTypedId.GetGenericArguments()[0];
            }

            stronglyTypedId = stronglyTypedId.BaseType!;
        }

        throw new InvalidOperationException();
    }
}