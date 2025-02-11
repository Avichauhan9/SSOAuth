using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Permissions;

public class GetAllPermission(IManagePermissionService managePermissionService) : EndpointBaseAsync
 .WithoutRequest
 .WithActionResult<GetAllPermissionDTO>
{
    private readonly IManagePermissionService _managePermissionService = managePermissionService;

    [HttpGet("permissions")]
    [SwaggerOperation(Summary = "Get all permissions", Description = "", OperationId = "Permissions.GetAllPermissions", Tags = ["Permissions"]
  )]
    public override async Task<ActionResult<GetAllPermissionDTO>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await _managePermissionService.GetAllPermissions();

        return result.ToActionResult(this);
    }
}