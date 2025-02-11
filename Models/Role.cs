using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("Roles")]
public class Role : BaseModel
{
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    public bool IsServicePrincipal { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

}
