
using Microsoft.AspNetCore.Authorization;
using SSO_Backend.Middlewares.Models;
using SSO_Backend.Services;

namespace SSO_Backend.Middlewares;

public class PermissionAuthHandler(IServiceProvider serviceProvider) : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated) return;

        using var scope = _serviceProvider.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        var userPermissions = permissionService.GetPermissions();
        List<string> permissions = [.. requirement.Permission.Split(',')];

        foreach (var permission in permissions)
        {
            if (userPermissions.Permissions.Contains(permission))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
