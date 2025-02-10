using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Users;

public class UpdateUserStatus(IManageUserService manageUserService) : EndpointBaseAsync
 .WithRequest<UpdateUserStatusDTO>
 .WithActionResult<UserDTO>
{
    private readonly IManageUserService _manageUserService = manageUserService;

    [HttpPatch("users/update-status")]
    [SwaggerOperation(Summary = "Update user status", Description = "", OperationId = "Users.UpdateUserStatus", Tags = ["Users"]
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync([FromBody] UpdateUserStatusDTO changeRequest, CancellationToken cancellationToken = default)
    {
        var result = await _manageUserService.ModifyUserStatus(changeRequest);

        return result.ToActionResult(this);
    }
}
