
using Microsoft.Graph.Models;
using SSO_Backend.Models;
namespace SSO_Backend.Services;

public interface IGraphService
{
    public Task<SignInCollectionResponse?> GetSignInsLogs(string signInEventType);
    public Task<Invitation?> InviteGuestUser(Models.User user);
    public Task<AppRoleAssignment?> UserAssignment(string userId);
    public Task UpdateUser(Models.User user);
    public Task DeleteUserByAzureADId(string azureAdId);
    public Task DisabledUser(string azureADUserId, bool accountEnabled);
    public Task<Microsoft.Graph.Models.User?> GetUserByEmail(string email);
}
