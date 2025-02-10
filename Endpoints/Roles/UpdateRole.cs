using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Roles.DTO;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Roles;

public class UpdateRole(IManageRoleService manageRoleService) : EndpointBaseAsync
 .WithRequest<UpdateRoleDTO>
 .WithActionResult<UserDTO>
{
    private readonly IManageRoleService _manageRoleService = manageRoleService;

    [HttpPut("roles/update")]
    [SwaggerOperation(Summary = "Update role information", Description = "", OperationId = "Roles.UpdateRole", Tags = ["Roles"]
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync([FromBody] UpdateRoleDTO updateUserRequest, CancellationToken cancellationToken = default)
    {
        var result = await _manageRoleService.UpdateRole(updateUserRequest);

        return result.ToActionResult(this);
    }
}
