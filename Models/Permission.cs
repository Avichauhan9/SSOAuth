
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("Permissions")]
public class Permission : BaseModel
{
    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public required string Code { get; set; }

    public bool IsServicePrincipal { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}
