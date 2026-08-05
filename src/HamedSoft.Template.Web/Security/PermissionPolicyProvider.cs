using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace HamedSoft.Template.Web.Security;

public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(
        IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionAttribute.PolicyPrefix))
        {
            var permission = policyName[
                PermissionAttribute.PolicyPrefix.Length..];

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return policy;
        }

        return await base.GetPolicyAsync(policyName);
    }
}