using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;
using SSO_Backend.Middlewares;

namespace SSO_Backend.Endpoints.Users;


public class Me(IManageUserService manageUserService) : EndpointBaseAsync
 .WithoutRequest
 .WithActionResult<UserDTO>
{
  private readonly IManageUserService _manageUserService = manageUserService;

  [HttpGet("users/me")]
  [SwaggerOperation(Summary = "Get information of current user", Description = "", OperationId = "Users.Me", Tags = new[] { "Users" }
)]
  public override async Task<ActionResult<UserDTO>> HandleAsync(CancellationToken cancellationToken = default)
  {
    var result = await _manageUserService.GetUserByEmail(CurrentUser.GetUniqueIdentityParameter(User));

    return result.ToActionResult(this);
  }
}