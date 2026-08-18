namespace HamedSoft.Template.Web.ErrorHandling;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddExceptionHandling(
        this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}