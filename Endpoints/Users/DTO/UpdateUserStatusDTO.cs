

namespace SSO_Backend.Endpoints.Users.DTO;

public class UpdateUserStatusDTO
{
    public required int Id { get; set; }

    public required bool AccountEnabled { get; set; }
}
