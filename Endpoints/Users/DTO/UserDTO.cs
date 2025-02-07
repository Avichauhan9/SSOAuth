

using SSO_Backend.Endpoints.Users.DTO;

namespace SSO_Backend.Endpoints.Users.DTO;

public class UserDTO : AllUserDTO
{
    public ICollection<string> Permissions { get; set; } = [];
    public ICollection<string> Roles { get; set; } = [];
}
