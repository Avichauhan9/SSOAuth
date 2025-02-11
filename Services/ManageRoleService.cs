using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSO_Backend.Context;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Models;
using SSO_Backend.Models.AppSettings;
using SSO_Backend.Result;

namespace SSO_Backend.Services;

public class ManageRoleService(AppDBContext DBContext, IMapper mapper, UserInfo user) : BaseService(DBContext, user), IManageRoleService
{
    private readonly IMapper _mapper = mapper;
    public async Task<Result<IEnumerable<AllRolesDTO>>> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Result<IEnumerable<AllRolesDTO>>.Success(_mapper.Map<IEnumerable<AllRolesDTO>>(roles));
    }

    public async Task<Result<int>> CreateRole(CreateRoleDTO request)
    {
        var existingRole = await _context.Roles.Include(r=>r.RolePermissions).
            ThenInclude(rp=>rp.Permission).
            SingleOrDefaultAsync(r => r.Name.ToLower() == request.Name.ToLower());
        if (existingRole != null)
            return Result<int>.Invalid([new ValidationError { Key = "Role", ErrorMessage = $"Role with name {request.Name} already exists." }]);
        var role = _mapper.Map<Role>(request);
        role.CreatedById = _user.Id;
        role.UpdatedById = _user.Id;
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        if (request.Permissions.Length > 0)
        {
            var rolePermissions = request.Permissions.Select(permissionId => new RolePermission { RoleId = role.Id, PermissionId = permissionId, CreatedById = _user.Id, UpdatedById = _user.Id }).ToList();
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
        }
        return Result<int>.Success(role.Id);
    }

    public async Task<Result<RoleDTO>> GetRoleById(int roleId)
    {
        var existingRole = await _context.Roles.Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission).SingleOrDefaultAsync(r => r.Id == roleId);
        if (existingRole == null)
            return Result<RoleDTO>.Invalid([new ValidationError { Key = "Role", ErrorMessage = $"Role is not found." }]);
        return Result<RoleDTO>.Success(_mapper.Map<RoleDTO>(existingRole));
    }

    public async Task<Result<int>> UpdateRole(UpdateRoleDTO request)
    {
        var existingRole = await _context.Roles.Include(r=>r.RolePermissions).SingleOrDefaultAsync(r => r.Id == request.Id);
        if (existingRole == null)
            return Result<int>.Invalid([new ValidationError { Key = "Role", ErrorMessage = $"Role is not found." }]);
        existingRole.Name = request.Name;
        existingRole.Description = request.Description;
        existingRole.UpdatedById = _user.Id;
        _context.Roles.Update(existingRole);
        _context.RolePermissions.RemoveRange(existingRole.RolePermissions);
        if (request.PermissionIds.Length > 0)
        {
            var rolePermissions = request.PermissionIds.Select(permissionId => new RolePermission { RoleId = existingRole.Id, PermissionId = permissionId, CreatedById = _user.Id, UpdatedById = _user.Id }).ToList();
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
        }
        await _context.SaveChangesAsync();
        return Result<int>.Success(200);
    }

    public async Task<Result<IEnumerable<UserByRoleDTO>>> GetUsersByRole(int roleId)
    {
        //complete full implematation.
        var existingRole = await _context.Roles.Include(r => r.UserRoles).ThenInclude(rp => rp.User).SingleOrDefaultAsync(r => r.Id == roleId);
        if (existingRole == null)
            return Result<IEnumerable<UserByRoleDTO>>.Invalid([new ValidationError { Key = "Role", ErrorMessage = $"Role is not found." }]);
        var users = existingRole.UserRoles.Select(ur => ur.User).ToList();

        return Result<IEnumerable<UserByRoleDTO>>.Success(_mapper.Map<IEnumerable<UserByRoleDTO>>(users));

    }
   
}
