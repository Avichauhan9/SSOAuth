using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Permissions.DTO
{
    public class CreatePermissionDTO
    {
        [Required]
        [StringLength(100)]
        public string PermissionName { get; set; }

        [StringLength(100)]
        public string PermissionCode { get; set; }

        public bool IsServicePrincipal { get; set; } = false;
    }
}
