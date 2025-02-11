using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Permissions;

public class GetRolesByPermission(IManagePermissionService managePermissionService) : EndpointBaseAsync
 .WithRequest<int>
 .WithActionResult<RolesByPermissionDTO>
{
  private readonly IManagePermissionService _managePermissionService = managePermissionService;

  [HttpGet("permissions/roles/{permissionId:int}")]
  [SwaggerOperation(Summary = "Get all roles associated with permission", Description = "", OperationId = "Permissions.GetRolesByPermission", Tags = ["Permissions"]
)]
  public override async Task<ActionResult<RolesByPermissionDTO>> HandleAsync(int permissionId, CancellationToken cancellationToken = default)
  {
    var result = await _managePermissionService.GetRolesByPermission(permissionId);

    return result.ToActionResult(this);
  }
}