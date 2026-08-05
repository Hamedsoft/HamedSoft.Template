using System.Security.Claims;
using HamedSoft.Template.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;

namespace HamedSoft.Template.Infrastructure.Identity.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User.Identity?.Name;

    public IReadOnlyCollection<string> Roles => _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray() ?? Array.Empty<string>();

    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}
