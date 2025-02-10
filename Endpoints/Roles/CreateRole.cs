using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Roles;

public class CreateRole(IManageRoleService manageRoleService) : EndpointBaseAsync
 .WithRequest<CreateRoleDTO>
 .WithActionResult<int>
{
    private readonly IManageRoleService _manageRoleService = manageRoleService;

    [HttpPost("roles/create")]
    [SwaggerOperation(Summary = "Create new role", Description = "", OperationId = "Roles.Create", Tags = ["Roles"]
  )]
    public override async Task<ActionResult<int>> HandleAsync(CreateRoleDTO newRoleRequest, CancellationToken cancellationToken = default)
    {
        var result = await _manageRoleService.CreateRole(newRoleRequest);

        return result.ToActionResult(this);
    }
}