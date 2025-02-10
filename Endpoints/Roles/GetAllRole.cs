using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Roles;

public class GetAllRole(IManageRoleService manageRoleService) : EndpointBaseAsync
 .WithoutRequest
 .WithActionResult<AllRolesDTO>
{
    private readonly IManageRoleService _manageRoleService = manageRoleService;

    [HttpGet("roles")]
    [SwaggerOperation(Summary = "Get all roles", Description = "", OperationId = "Roles.GetAllRole", Tags = ["Roles"]
  )]
    public override async Task<ActionResult<AllRolesDTO>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await _manageRoleService.GetRoles();

        return result.ToActionResult(this);
    }
}