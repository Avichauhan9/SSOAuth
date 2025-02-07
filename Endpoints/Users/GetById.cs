using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;
using SSO_Backend.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SSO_Backend.Endpoints.Users;


public class GetById(IManageUserService manageUserService) : EndpointBaseAsync
 .WithRequest<int>
 .WithActionResult<UserDTO>
{
    private readonly IManageUserService _manageUserService = manageUserService;

    [HttpGet("users/{id}")]
    [SwaggerOperation(Summary = "Get user by id", Description = "", OperationId = "Users.GetById", Tags = new[] { "Users" }
  )]
    public override async Task<ActionResult<UserDTO>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _manageUserService.GetUserById(id);

        return result.ToActionResult(this);
    }
}