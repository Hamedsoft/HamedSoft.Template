namespace HamedSoft.Template.Application;

/// <summary>
/// Marker type used for assembly scanning.
///
/// This class provides a stable reference to the Application assembly for
/// libraries that discover services via reflection, such as MediatR,
/// FluentValidation, AutoMapper, Scrutor, etc.
///
/// Example:
/// services.AddMediatR(cfg =>
///     cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
/// </summary>
public sealed class AssemblyReference;