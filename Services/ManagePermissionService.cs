using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSO_Backend.Context;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Models;
using SSO_Backend.Result;

namespace SSO_Backend.Services;

public class ManagePermissionService(AppDBContext DBContext, IMapper mapper, UserInfo user) : BaseService(DBContext, user), IManagePermissionService
{
    private readonly IMapper _mapper = mapper;

    public async Task<Result<int>> CreatePermission(CreatePermissionDTO request)
    {
        var existingPermission = await _context.Permissions.SingleOrDefaultAsync(p => p.Name.ToLower() == request.PermissionName.ToLower());
        if (existingPermission != null)
            return Result<int>.Invalid([new() { Key = "Permission", ErrorMessage = $"Permission with name {request.PermissionName} already exists." }]);

        var permission = _mapper.Map<Permission>(request);
        permission.CreatedById = _user.Id;
        permission.UpdatedById = _user.Id;
        await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
        return Result<int>.Success(permission.Id);
    }

    public async Task<Result<IEnumerable<GetAllPermissionDTO>>> GetAllPermissions()
    {
        var permissions = await _context.Permissions.ToListAsync();
        return Result<IEnumerable<GetAllPermissionDTO>>.Success(_mapper.Map<IEnumerable<GetAllPermissionDTO>>(permissions));
    }

    public async Task<Result<PermissionByIdDTO>> GetPermissionById(int permissionId)
    {
        var permission = await _context.Permissions.FindAsync(permissionId);
        return permission == null ? Result<PermissionByIdDTO>.Invalid([new() { Key = "Permission", ErrorMessage = $"Permission is not found." }]) : Result<PermissionByIdDTO>.Success(_mapper.Map<PermissionByIdDTO>(permission));
    }

    public Task<Result<IEnumerable<RolesByPermissionDTO>>> GetRolesByPermission(int permissionId)
    {
        var roles = _context.RolePermissions.Include(rp => rp.Role).Where(rp => rp.PermissionId == permissionId).Select(rp => rp.Role).ToList();
        return Task.FromResult(Result<IEnumerable<RolesByPermissionDTO>>.Success(_mapper.Map<IEnumerable<RolesByPermissionDTO>>(roles)));
    }
}
