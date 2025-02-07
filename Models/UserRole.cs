
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("UserRoles")]
public class UserRole : BaseModel
{

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]

    public virtual Role Role { get; set; }
}
