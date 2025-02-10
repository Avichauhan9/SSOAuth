namespace SSO_Backend.Endpoints.Roles.DTO
{
    public class RoleDTO :AllRolesDTO
    {
        public PermissionDTO[] Permissions { get; set; } = [];

    }

    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
