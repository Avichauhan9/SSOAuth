
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("RolePermissions")]
public class RolePermission : BaseModel
{

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role Role { get; set; }

    public int PermissionId { get; set; }

    [ForeignKey("PermissionId")]
    public virtual Permission Permission { get; set; }
}


