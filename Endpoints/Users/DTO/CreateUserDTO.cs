
using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Users.DTO;

public class CreateUserDTO
{
    [MaxLength(300)]
    [EmailAddress]
    public required string Email { get; set; }


    [MaxLength(100)]
    public required string FirstName { get; set; }

    [MaxLength(100)]

    public required string LastName { get; set; }

    public List<int>? RoleIds { get; set; }

}
