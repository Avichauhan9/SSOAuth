
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("Permissions")]
public class Permission : BaseModel
{
    public required string Name { get; set; }

    public required string Code { get; set; }

    public bool IsServicePrincipal { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}
