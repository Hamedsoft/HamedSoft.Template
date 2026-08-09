using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HamedSoft.Template.Web.Security;

[HtmlTargetElement("permission", Attributes = "require")]
public sealed class PermissionTagHelper : TagHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionTagHelper(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Require { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null || !user.HasPermission(Require))
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = null;
    }
}