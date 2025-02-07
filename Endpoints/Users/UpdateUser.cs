using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Users;

public class UpdateUser(IManageUserService manageUserService) : EndpointBaseAsync
 .WithRequest<UpdateUserDTO>
 .WithActionResult<UserDTO>
{
    private readonly IManageUserService _manageUserService = manageUserService;

    [HttpPut("users/update")]
    [SwaggerOperation(Summary = "Update user information", Description = "", OperationId = "Users.UpdateUser", Tags = new[] { "Users" }
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync([FromBody] UpdateUserDTO updateUserRequest, CancellationToken cancellationToken = default)
    {
        var result = await _manageUserService.UpdateUser(updateUserRequest);

        return result.ToActionResult(this);
    }
}
