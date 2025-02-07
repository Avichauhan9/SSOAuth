
using Microsoft.AspNetCore.Authorization;

namespace SSO_Backend.Middlewares.Models;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
