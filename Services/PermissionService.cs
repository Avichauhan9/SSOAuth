
using SSO_Backend.Middlewares.Models;

namespace SSO_Backend.Services;

public class PermissionService(UserInfo user) : AuthService(user), IPermissionService
{
    public PermissionVM GetPermissions()
    {
        return new PermissionVM
        {
            Permissions = _user.Permissions,
            Roles = _user.Roles
        };
    }
}
