
using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Users.DTO;

public class UpdateUserDTO
{
    public required int Id { get; set; }
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]

    public string? LastName { get; set; }

    public bool? IsServicePrincipal { get; set; }

    public List<int>? RoleIds { get; set; }
}
