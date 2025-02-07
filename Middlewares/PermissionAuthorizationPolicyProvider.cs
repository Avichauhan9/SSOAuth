
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SSO_Backend.Middlewares.Models;

namespace SSO_Backend.Middlewares;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? authorizationPolicy = await base.GetPolicyAsync(policyName);
        if (authorizationPolicy is not null)
            return authorizationPolicy;
        return new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();
    }
}
