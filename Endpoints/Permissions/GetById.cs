using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Permissions.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Permissions;

public class GetRoleById(IManagePermissionService managePermissionService) : EndpointBaseAsync
 .WithRequest<int>
 .WithActionResult<PermissionByIdDTO>
{
    private readonly IManagePermissionService _managePermissionService = managePermissionService;

    [HttpGet("permissions/{id}")]
    [SwaggerOperation(Summary = "Get permission by id", Description = "", OperationId = "Permissions.GetById", Tags = ["Permissions"]
  )]
    public override async Task<ActionResult<PermissionByIdDTO>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _managePermissionService.GetPermissionById(id);
        return result.ToActionResult(this);
    }
}