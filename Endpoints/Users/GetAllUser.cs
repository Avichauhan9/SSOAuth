using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Users;


public class GetAllUser(IManageUserService manageUserService) : EndpointBaseAsync
 .WithRequest<bool?>
 .WithActionResult<UserDTO>
{
    private readonly IManageUserService _manageUserService = manageUserService;

    [HttpGet("users")]
    [SwaggerOperation(Summary = "Get all users", Description = "", OperationId = "Users.GetAllUser", Tags = new[] { "Users" }
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync([FromQuery] bool? IsActive, CancellationToken cancellationToken = default)
    {
        var result = await _manageUserService.GetUsers(IsActive);

        return result.ToActionResult(this);
    }
}