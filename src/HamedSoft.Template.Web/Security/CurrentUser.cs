using System.Security.Claims;
using HamedSoft.Template.Application.Contracts.Security;

namespace HamedSoft.Template.Web.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated
        ?? false;

    public Guid? UserId
    {
        get
        {
            var value = User?.FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string? UserName =>
        User?.FindFirstValue(ClaimTypes.Name);

    public string? DisplayName =>
        User?.FindFirstValue(CustomClaimTypes.DisplayName);

    public IReadOnlyCollection<string> Roles =>
        User?
            .FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray()
        ?? Array.Empty<string>();

    public bool IsInRole(string role) =>
        User?.IsInRole(role) ?? false;
}