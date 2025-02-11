using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Permissions;

public class CreatePermission(IManagePermissionService managePermissionService) : EndpointBaseAsync
 .WithRequest<CreatePermissionDTO>
 .WithActionResult<int>
{
    private readonly IManagePermissionService _managePermissionService = managePermissionService;

    [HttpPost("permissions/create")]
    [SwaggerOperation(Summary = "Create new permission", Description = "", OperationId = "Permissions.Create", Tags = ["Permissions"]
  )]
    public override async Task<ActionResult<int>> HandleAsync(CreatePermissionDTO newRoleRequest, CancellationToken cancellationToken = default)
    {
        var result = await _managePermissionService.CreatePermission(newRoleRequest);

        return result.ToActionResult(this);
    }
}