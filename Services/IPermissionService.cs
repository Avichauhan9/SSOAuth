
using SSO_Backend.Middlewares.Models;

namespace SSO_Backend.Services;

public interface IPermissionService
{
    PermissionVM GetPermissions();
}
