using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Roles.DTO
{
    public class UpdateRoleDTO
    {

        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public bool IsServicePrincipal { get; set; }

        public int[] PermissionIds { get; set; } = [];
    }
}
