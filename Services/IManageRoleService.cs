using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Result;

namespace SSO_Backend.Services
{
    public interface IManageRoleService
    {
        public Task<Result<IEnumerable<AllRolesDTO>>> GetRoles();
        public Task<Result<RoleDTO>> GetRoleById(int roleId);
        public Task<Result<IEnumerable<UserByRoleDTO>>> GetUsersByRole(int roleId);
        public Task<Result<int>> CreateRole(CreateRoleDTO request);
        public Task<Result<int>> UpdateRole(UpdateRoleDTO request);
    }
}
