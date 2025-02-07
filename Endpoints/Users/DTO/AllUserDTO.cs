namespace SSO_Backend.Endpoints.Users.DTO;

public class AllUserDTO
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Name => $"{FirstName} {LastName}";

    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public bool IsServicePrincipal { get; set; }
}