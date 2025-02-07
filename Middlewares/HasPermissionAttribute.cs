
using Microsoft.AspNetCore.Authorization;

namespace SSO_Backend.Middlewares;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permissions) : base(policy: permissions)
    {
    }
}
