using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Roles;

public class GetRoleById(IManageRoleService manageRoleService) : EndpointBaseAsync
 .WithRequest<int>
 .WithActionResult<RoleDTO>
{
    private readonly IManageRoleService _manageRoleService = manageRoleService;

    [HttpGet("roles/{id}")]
    [SwaggerOperation(Summary = "Get role by id", Description = "", OperationId = "Roles.GetById", Tags = ["Roles"]
  )]
    public override async Task<ActionResult<RoleDTO>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _manageRoleService.GetRoleById(id);
        return result.ToActionResult(this);
    }
}