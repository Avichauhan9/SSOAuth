using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Roles;

public class GetUsersByRole(IManageRoleService manageRoleService) : EndpointBaseAsync
 .WithRequest<int>
 .WithActionResult<UserByRoleDTO>
{
    private readonly IManageRoleService _manageRoleService = manageRoleService;

    [HttpGet("roles/users/${roleId:int}")]
    [SwaggerOperation(Summary = "Get all users by role", Description = "", OperationId = "Roles.GetAllRole", Tags = ["Roles"]
  )]
    public override async Task<ActionResult<UserByRoleDTO>> HandleAsync(int roleId,CancellationToken cancellationToken = default)
    {
        var result = await _manageRoleService.GetUsersByRole(roleId);

        return result.ToActionResult(this);
    }
}