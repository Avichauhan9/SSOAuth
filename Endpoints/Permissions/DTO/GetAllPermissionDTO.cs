
namespace SSO_Backend.Endpoints.Permissions.DTO
{
    public class GetAllPermissionDTO
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public string PermissionCode { get; set; }


        public bool IsServicePrincipal { get; set; }
    }
}
