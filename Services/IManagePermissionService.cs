using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Result;

namespace SSO_Backend.Services
{
    public interface IManagePermissionService
    {
        public Task<Result<IEnumerable<RolesByPermissionDTO>>> GetRolesByPermission(int permissionId);
        public Task<Result<int>> CreatePermission(CreatePermissionDTO request);
        public Task<Result<IEnumerable<GetAllPermissionDTO>>> GetAllPermissions();
        public Task<Result<PermissionByIdDTO>> GetPermissionById(int permissionId);

    }
}
