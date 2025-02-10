using System.ComponentModel.DataAnnotations;

namespace SSO_Backend.Endpoints.Roles.DTO
{
    public class CreateRoleDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool IsServicePrincipal { get; set; } = false;

        public int[] Permissions { get; set; } = [];
    }
}
