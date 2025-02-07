using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("Roles")]
public class Role : BaseModel
{
    public required string Name { get; set; }
    public bool IsServicePrincipal { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

}
