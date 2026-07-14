namespace HamedSoft.Template.Domain.SeedWork;

public static class Guard
{
    public static void AgainstNull(
        object? value,
        string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);
    }

    public static void AgainstNullOrWhiteSpace(
        string? value,
        string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                $"{parameterName} cannot be empty.",
                parameterName);
    }

    public static void AgainstDefault<T>(
        T value,
        string parameterName)
        where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException(
                $"{parameterName} cannot be default.",
                parameterName);
    }
}