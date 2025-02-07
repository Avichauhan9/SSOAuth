using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Backend.Models;

[Table("Users")]
public class User : BaseModel
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }

    public required string Email { get; set; }

    public string? AzureADUserId { get; set; }

    public bool IsServicePrincipal { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

}
