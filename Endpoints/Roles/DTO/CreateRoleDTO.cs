using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Roles.DTO
{
    public class CreateRoleDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IsServicePrincipal { get; set; } = false;

        public int[] Permissions { get; set; } = [];
    }
}
