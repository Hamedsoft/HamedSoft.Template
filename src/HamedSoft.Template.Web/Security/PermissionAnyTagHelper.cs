using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HamedSoft.Template.Web.Security;

[HtmlTargetElement("permission-any", Attributes = "require")]
public sealed class PermissionAnyTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionAnyTagHelper(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Require { get; set; } = string.Empty;

    public override void Process(
        TagHelperContext context,
        TagHelperOutput output)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
        {
            output.SuppressOutput();
            return;
        }

        var permissions = Require
            .Split(
                ',',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

        if (!user.HasAnyPermission(permissions))
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = null;
    }
}