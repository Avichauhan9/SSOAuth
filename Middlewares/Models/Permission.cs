

namespace SSO_Backend.Middlewares.Models;

public class PermissionVM
{
    public HashSet<string> Permissions { get; set; }

    public HashSet<string> Roles { get; set; }

    public PermissionVM()
    {
        Permissions = [];
        Roles = [];
    }

}

public static class Permissions
{
    public const string Read = nameof(Read);
    public const string Write = nameof(Write);
    public const string Delete = nameof(Delete);
}