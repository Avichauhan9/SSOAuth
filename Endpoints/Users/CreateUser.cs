using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Users;

public class CreateUser(IManageUserService manageUserService) : EndpointBaseAsync
 .WithRequest<CreateUserDTO>
 .WithActionResult<UserDTO>
{
    private readonly IManageUserService _manageUserService = manageUserService;

    [HttpPost("users/create")]
    [SwaggerOperation(Summary = "Create new user", Description = "", OperationId = "Users.Create", Tags = ["Users"]
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync(CreateUserDTO userRequest, CancellationToken cancellationToken = default)
    {
        var result = await _manageUserService.CreateUser(userRequest);

        return result.ToActionResult(this);
    }
}