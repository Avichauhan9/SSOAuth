
using SSO_Backend.Endpoints.Users.DTO;
using SSO_Backend.Result;

namespace SSO_Backend.Services;

public interface IManageUserService
{
    public Task<Result<IEnumerable<AllUserDTO>>> GetUsers(bool? isActive);
    public Task<Result<UserDTO>> GetUserByEmail(string email);
    public Task<Result<UserDTO>> GetUserById(int id);
    public Task<Result<UserDTO>> CreateUser(CreateUserDTO user);
    public Task<Result<int>> UpdateUser(UpdateUserDTO user);
    public Task<Result<int>> ModifyUserStatus(UpdateUserStatusDTO changeStatusRequest);
}
